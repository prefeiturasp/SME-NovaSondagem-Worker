using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.NovaSondagem.Infra.Fila;

namespace SME.NovaSondagem.Infra.Interfaces;

public interface IRabbitMqMessageProcessor
{
    Task ProcessMessageAsync(BasicDeliverEventArgs ea, IChannel channel, Dictionary<string, ComandoRabbit> comandos);
}
