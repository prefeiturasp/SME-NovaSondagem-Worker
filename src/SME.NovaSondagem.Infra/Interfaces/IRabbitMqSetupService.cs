using RabbitMQ.Client;
using SME.NovaSondagem.Infra.Fila;

namespace SME.NovaSondagem.Infra.Interfaces;

public interface IRabbitMqSetupService
{
    Task<IConnection> CreateConnectionAsync(CancellationToken stoppingToken);
    Task SetupExchangesAndQueuesAsync(IChannel channel, Dictionary<string, ComandoRabbit> comandos);
}
