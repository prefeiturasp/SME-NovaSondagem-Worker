using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using Polly.Registry;
using SME.NovaSondagem.Infra.Fila;
using SME.NovaSondagem.Infra.Policies;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Infra.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Services;

public class ServicoMensageriaTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_dependencias_nulas()
    {
        var rabbitOptions = new RabbitOptions
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/"
        };
        var telemetria = new Mock<IServicoTelemetria>().Object;
        var logger = new Mock<ILogger<ServicoMensageria>>().Object;
        var registry = new PolicyRegistry
        {
            [PoliticaPolly.PublicaFila] = Policy.NoOpAsync()
        };

        Assert.Throws<ArgumentNullException>(() => new ServicoMensageria(null!, telemetria, registry, logger));
        Assert.Throws<ArgumentNullException>(() => new ServicoMensageria(rabbitOptions, null!, registry, logger));
        Assert.Throws<NullReferenceException>(() => new ServicoMensageria(rabbitOptions, telemetria, null!, logger));
        Assert.Throws<ArgumentNullException>(() => new ServicoMensageria(rabbitOptions, telemetria, registry, null!));
    }
}
