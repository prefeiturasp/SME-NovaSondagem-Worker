using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Dominio.Enums;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Extensions;
using SME.NovaSondagem.Infra.Fila;
using SME.NovaSondagem.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Services
{
    public class ServicoLog : IServicoLog
    {
        private readonly ILogger<ServicoLog> logger;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly RabbitLogOptions configuracaoRabbitOptions;
        public ServicoLog(IServicoTelemetria servicoTelemetria, RabbitLogOptions configuracaoRabbitOptions, ILogger<ServicoLog> logger)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Registrar(Exception ex)
        {
            LogMensagem logMensagem = new LogMensagem("Exception --- ", LogNivel.Critico, ex.Message, ex.StackTrace);
            Registrar(logMensagem);
        }

        public void Registrar(LogNivel nivel, string erro, string observacoes, string stackTrace)
        {
            LogMensagem logMensagem = new LogMensagem(erro, nivel, observacoes, stackTrace);
            Registrar(logMensagem);

        }

        public void Registrar(string mensagem, Exception ex)
        {
            LogMensagem logMensagem = new LogMensagem(mensagem, LogNivel.Critico, ex.Message, ex.StackTrace);

            Registrar(logMensagem);
        }
        private void Registrar(LogMensagem log)
        {
            var body = Encoding.UTF8.GetBytes(log.ConverterObjectParaJson());
            servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbit.RotaLogs);
        }

        private async void PublicarMensagem(byte[] body)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuracaoRabbitOptions.HostName,
                    UserName = configuracaoRabbitOptions.UserName,
                    Password = configuracaoRabbitOptions.Password,
                    VirtualHost = configuracaoRabbitOptions.VirtualHost
                };

                using var conexaoRabbit = await factory.CreateConnectionAsync();
                using var channel = await conexaoRabbit.CreateChannelAsync();
                var props = new BasicProperties
                {
                    Persistent = true
                };

                await channel.BasicPublishAsync(
                    ExchangeRabbit.Logs,
                    RotasRabbit.RotaLogs,
                    true,
                    props,
                    body
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}