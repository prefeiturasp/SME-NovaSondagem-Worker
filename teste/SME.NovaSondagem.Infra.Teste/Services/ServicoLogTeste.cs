using Microsoft.Extensions.Logging;
using Moq;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Infra.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Services;

public class ServicoLogTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_dependencias_nulas()
    {
        var rabbitLog = new RabbitLogOptions
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/"
        };
        var telemetria = new Mock<IServicoTelemetria>().Object;
        var logger = new Mock<ILogger<ServicoLog>>().Object;

        Assert.Throws<ArgumentNullException>(() => new ServicoLog(null!, rabbitLog, logger));
        Assert.Throws<ArgumentNullException>(() => new ServicoLog(telemetria, null!, logger));
        Assert.Throws<ArgumentNullException>(() => new ServicoLog(telemetria, rabbitLog, null!));
    }
}
