using Microsoft.Extensions.Logging;
using Moq;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Worker.Mensageria;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Worker.Teste.Mensageria;

public class RabbitMqConsumerServiceTeste
{
    [Fact]
    public void Deve_lancar_not_supported_no_registro_de_usecases()
    {
        var logger = new Mock<ILogger<RabbitMqConsumerService>>().Object;
        var servicoLog = new Mock<IServicoLog>().Object;
        var servicoMensageria = new Mock<IServicoMensageria>().Object;
        var setupService = new Mock<IRabbitMqSetupService>().Object;
        var processor = new Mock<IRabbitMqMessageProcessor>().Object;

        var acao = () => new RabbitMqConsumerService(logger, servicoLog, servicoMensageria, setupService, processor);

        Assert.Throws<NotSupportedException>(acao);
    }
}
