using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.NovaSondagem.Worker.Dtos;
using SME.NovaSondagem.Worker.Interfaces.Repositories;
using SME.NovaSondagem.Worker.Options;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Workers.Worker
{
    public abstract class WorkerRabbitSondagem : IHostedService, IDisposable
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConsumoFilasOptions _consumoFilasOptions;
        private readonly IConnectionFactory _connectionFactory;
        private readonly string _nomeWorker;
        protected readonly ILogger<WorkerRabbitSondagem> _logger;
        
        protected IConnection? conexaoRabbit;
        protected IChannel? canalRabbit;
        protected readonly ConcurrentDictionary<string, IRabbitUseCase> Comandos = new();

        protected WorkerRabbitSondagem(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory connectionFactory,
            string nomeWorker,
            Type tipoRotas,
            ILogger<WorkerRabbitSondagem> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _consumoFilasOptions = consumoFilasOptions.Value;
            _connectionFactory = connectionFactory;
            _nomeWorker = nomeWorker;
            _logger = logger;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("=== INICIANDO {Worker} ===", _nomeWorker);
                
                // Debug das configurações do ConnectionFactory
                await LogConnectionFactoryInfo();
                
                await IniciarConexaoRabbit();
                
                _logger.LogInformation("Registrando Use Cases...");
                RegistrarUseCases();
                
                _logger.LogInformation("Configurando consumidores para {Count} comandos...", Comandos.Count);
                await ConfigurarConsumidores();
                
                _logger.LogInformation("=== {Worker} INICIADO COM SUCESSO ===", _nomeWorker);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERRO CRÍTICO ao iniciar {Worker}: {Message}", _nomeWorker, ex.Message);
                throw;
            }
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Parando {Worker}...", _nomeWorker);
            await FecharConexoes();
            _logger.LogInformation("{Worker} parado.", _nomeWorker);
        }

        protected virtual async Task LogConnectionFactoryInfo()
        {
            try
            {
                // Extrai host e porta da Uri do ConnectionFactory
                var uri = _connectionFactory.Uri;
                var host = uri.Host;
                var port = uri.Port;

                // Log das configurações (sem revelar senha)
                _logger.LogInformation("=== CONFIGURAÇÕES RABBITMQ ===");
                _logger.LogInformation("HostName: {HostName}", host);
                _logger.LogInformation("Port: {Port}", port);
                _logger.LogInformation("UserName: {UserName}", _connectionFactory.UserName);
                _logger.LogInformation("VirtualHost: {VirtualHost}", _connectionFactory.VirtualHost);
                _logger.LogInformation("RequestedHeartbeat: {Heartbeat}s", _connectionFactory.RequestedHeartbeat.TotalSeconds);
                _logger.LogInformation("===========================");

                // Teste de conectividade básica
                _logger.LogInformation("Testando conectividade com {Host}:{Port}...", host, port);

                using var client = new System.Net.Sockets.TcpClient();
                await client.ConnectAsync(host, port);
                _logger.LogInformation("Conectividade TCP estabelecida com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRO na conectividade TCP: {Message}", ex.Message);
                _logger.LogWarning("Possíveis causas:");
                _logger.LogWarning("1. Servidor RabbitMQ não está rodando");
                _logger.LogWarning("2. Firewall bloqueando a conexão");
                _logger.LogWarning("3. Endereço/porta incorretos");
                throw new InvalidOperationException($"Não foi possível estabelecer conectividade TCP com {_connectionFactory.Uri.Host}:{_connectionFactory.Uri.Port}", ex);
            }
        }

        protected virtual async Task IniciarConexaoRabbit()
        {
            try
            {
                _logger.LogInformation("Criando conexão RabbitMQ...");
                conexaoRabbit = await _connectionFactory.CreateConnectionAsync();
                _logger.LogInformation("Conexão RabbitMQ criada com sucesso!");
                
                _logger.LogInformation("Criando canal RabbitMQ...");
                canalRabbit = await conexaoRabbit.CreateChannelAsync();
                _logger.LogInformation("Canal RabbitMQ criado com sucesso!");
                
                _logger.LogInformation("Configurando QoS (Qos: {Qos})...", _consumoFilasOptions.Qos);
                await canalRabbit.BasicQosAsync(0, _consumoFilasOptions.Qos, false);
                _logger.LogInformation("QoS configurado com sucesso!");
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex) when (ex.InnerException is RabbitMQ.Client.Exceptions.AuthenticationFailureException)
            {
                _logger.LogError("ERRO DE AUTENTICAÇÃO RABBITMQ");
                _logger.LogError("Mensagem: {Message}", ex.InnerException.Message);
                _logger.LogError("Verifique:");
                _logger.LogError("1. Usuário '{UserName}' existe no RabbitMQ", _connectionFactory.UserName);
                _logger.LogError("2. Senha está correta");
                _logger.LogError("3. Virtual Host '{VirtualHost}' existe", _connectionFactory.VirtualHost);
                _logger.LogError("4. Usuário tem permissões no Virtual Host");
                _logger.LogError("5. Execute: rabbitmqctl add_user {UserName} senha", _connectionFactory.UserName);
                _logger.LogError("6. Execute: rabbitmqctl set_permissions -p / {UserName} \".*\" \".*\" \".*\"", _connectionFactory.UserName);
                
                throw new InvalidOperationException($"Falha na autenticação RabbitMQ para usuário '{_connectionFactory.UserName}' no virtual host '{_connectionFactory.VirtualHost}'", ex);
            }
            catch (RabbitMQ.Client.Exceptions.AuthenticationFailureException ex)
            {
                _logger.LogError("ERRO DE AUTENTICAÇÃO RABBITMQ");
                _logger.LogError("Mensagem: {Message}", ex.Message);
                _logger.LogError("Verifique:");
                _logger.LogError("1. Usuário '{UserName}' existe no RabbitMQ", _connectionFactory.UserName);
                _logger.LogError("2. Senha está correta");
                _logger.LogError("3. Virtual Host '{VirtualHost}' existe", _connectionFactory.VirtualHost);
                _logger.LogError("4. Usuário tem permissões no Virtual Host");
                
                throw new InvalidOperationException($"Falha na autenticação RabbitMQ para usuário '{_connectionFactory.UserName}' no virtual host '{_connectionFactory.VirtualHost}'", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERRO ao iniciar conexão RabbitMQ: {Message}", ex.Message);
                throw;
            }
        }

        protected virtual async Task ConfigurarConsumidores()
        {
            if (canalRabbit == null)
            {
                _logger.LogWarning("Canal RabbitMQ é null - não é possível configurar consumidores");
                return;
            }

            foreach (var comando in Comandos)
            {
                try
                {
                    var queueName = comando.Key;
                    _logger.LogInformation("Declarando fila: {QueueName}", queueName);
                    
                    await canalRabbit.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                    
                    var consumer = new AsyncEventingBasicConsumer(canalRabbit);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        await ProcessarMensagem(ea, comando.Value);
                    };

                    await canalRabbit.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
                    
                    _logger.LogInformation("Consumidor configurado para fila: {QueueName}", queueName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERRO ao configurar consumidor para fila {QueueName}: {Message}", comando.Key, ex.Message);
                    throw;
                }
            }
        }

        protected virtual async Task ProcessarMensagem(BasicDeliverEventArgs ea, IRabbitUseCase useCase)
        {
            string? messageId = null;
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var mensagemRabbit = JsonSerializer.Deserialize<MensagemRabbit>(message);
                messageId = mensagemRabbit?.Id.ToString();

                _logger.LogDebug("Processando mensagem {MessageId} na fila {Queue}", messageId, ea.RoutingKey);

                if (mensagemRabbit != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    await useCase.Executar(mensagemRabbit);
                    _logger.LogDebug("Mensagem {MessageId} processada com sucesso", messageId);
                }

                if (canalRabbit != null)
                {
                    await canalRabbit.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    _logger.LogTrace("Mensagem {MessageId} confirmada (ACK)", messageId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERRO ao processar mensagem {MessageId}: {Message}", messageId, ex.Message);
                
                if (canalRabbit != null)
                {
                    await canalRabbit.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    _logger.LogWarning("Mensagem {MessageId} rejeitada e recolocada na fila", messageId);
                }
            }
        }

        protected abstract void RegistrarUseCases();

        protected virtual async Task FecharConexoes()
        {
            try
            {
                if (canalRabbit != null)
                {
                    _logger.LogInformation("Fechando canal RabbitMQ...");
                    await canalRabbit.CloseAsync();
                    canalRabbit.Dispose();
                    _logger.LogInformation("Canal RabbitMQ fechado");
                }

                if (conexaoRabbit != null)
                {
                    _logger.LogInformation("Fechando conexão RabbitMQ...");
                    await conexaoRabbit.CloseAsync();
                    conexaoRabbit.Dispose();
                    _logger.LogInformation("Conexão RabbitMQ fechada");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fechar conexões RabbitMQ: {Message}", ex.Message);
            }
        }

        public void Dispose()
        {
            FecharConexoes().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }
}