using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Worker.Mensageria;
using System.Text;
using Xunit;

namespace SME.NovaSondagem.Worker.Teste.Mensageria;

public class RabbitMqMessageProcessorFluxoTeste
{
    [Fact]
    public async Task Deve_rejeitar_mensagem_quando_rota_nao_esta_registrada()
    {
        var scopeFactory = new Mock<IServiceScopeFactory>().Object;
        var servicoTelemetria = new Mock<IServicoTelemetria>().Object;
        var servicoLog = new Mock<IServicoLog>().Object;
        var servicoMensageria = new Mock<IServicoMensageria>().Object;
        var logger = new Mock<ILogger<RabbitMqMessageProcessor>>().Object;
        var processor = new RabbitMqMessageProcessor(scopeFactory, servicoTelemetria, servicoLog, servicoMensageria, logger);

        var channel = new Mock<IChannel>();
        var evento = new BasicDeliverEventArgs(
            "consumer",
            10,
            false,
            "exchange",
            "rota.inexistente",
            new BasicProperties(),
            Encoding.UTF8.GetBytes("{\"Mensagem\":\"teste\"}"),
            CancellationToken.None);

        await processor.ProcessMessageAsync(evento, channel.Object, []);

        channel.Verify(c => c.BasicRejectAsync(10, false, It.IsAny<CancellationToken>()), Times.Once);
    }
}
