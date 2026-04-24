using Microsoft.Extensions.Logging;
using Moq;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Worker.Mensageria;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Worker.Teste.Mensageria;

public class RabbitMqMessageProcessorTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_service_scope_factory_for_nulo()
    {
        // arrange
        var servicoTelemetria = new Mock<IServicoTelemetria>().Object;
        var servicoLog = new Mock<IServicoLog>().Object;
        var servicoMensageria = new Mock<IServicoMensageria>().Object;
        var logger = new Mock<ILogger<RabbitMqMessageProcessor>>().Object;

        // act
        var acao = () => new RabbitMqMessageProcessor(
            null,
            servicoTelemetria,
            servicoLog,
            servicoMensageria,
            logger);

        // assert
        var excecao = Assert.Throws<ArgumentNullException>(acao);
        Assert.Equal("serviceScopeFactory", excecao.ParamName);
    }
}
