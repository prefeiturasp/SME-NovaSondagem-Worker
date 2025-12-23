using MediatR;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using SME.NovaSondagem.Dominio.Enums;
using SME.NovaSondagem.Infra.Extensions;
using SME.NovaSondagem.Infra.Fila;
using SME.NovaSondagem.Infra.Interfaces;
using System;
using System.Text;

namespace SME.NovaSondagem.Aplicacao.Commands.PublicaFilaRabbit
{
    public class PublicaFilaRabbitCommandHandler : IRequestHandler<PublicaFilaRabbitCommand, bool>
    {
        private readonly IChannel channel;
        private readonly IServicoLog servicoLog;

        public PublicaFilaRabbitCommandHandler(IChannel channel, IServicoLog servicoLog)
        {
            this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Handle(PublicaFilaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var body = Encoding.UTF8.GetBytes(mensagem.ConverterObjectParaJson());

                var props = new BasicProperties()
                {
                    Persistent = true
                };

                // Configurando política de retry com Polly
                AsyncRetryPolicy policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            servicoLog.Registrar(LogNivel.Critico.ToString(), new Exception($"Erro ao publicar mensagem, tentativa {retryCount}: {exception.Message}", exception));
                        });

                await policy.ExecuteAsync(async () =>
                {
                    var address = new PublicationAddress(ExchangeType.Direct, ExchangeRabbit.NovaSondagem, request.NomeRota);

                    await channel.BasicPublishAsync(address, props, body, cancellationToken);
                });

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(LogNivel.Critico, $"Erro: PublicaFilaRabbitCommand -- {ex.Message}",
                    $"Worker Serap: Rota -> {request.NomeRota} Fila -> {request.NomeFila}", ex.StackTrace);
                return false;
            }
        }
    }
}