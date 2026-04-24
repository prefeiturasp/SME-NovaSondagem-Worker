using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.IoC.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.IoC.Teste.Extensions;

public class RegistraDependenciasTeste
{
    [Fact]
    public void Deve_registrar_servicos_principais_da_infra()
    {
        var configuracao = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Rabbit:HostName"] = "localhost",
                ["Rabbit:UserName"] = "guest",
                ["Rabbit:Password"] = "guest",
                ["Rabbit:VirtualHost"] = "/",
                ["Rabbit:LimiteDeMensagensPorExecucao"] = "10",
                ["RabbitLog:HostName"] = "localhost",
                ["RabbitLog:UserName"] = "guest",
                ["RabbitLog:Password"] = "guest",
                ["RabbitLog:VirtualHost"] = "/",
                ["Telemetria:ApplicationInsights"] = "false",
                ["Telemetria:Apm"] = "false",
                ["UrlApiSondagem"] = "https://localhost",
                ["ApiKeySondagemApi"] = "chave-sondagem",
                ["UrlApiEOL"] = "https://localhost",
                ["ApiKeyEolApi"] = "chave-eol"
            })
            .Build();

        var services = new ServiceCollection();

        RegistraDependencias.Registrar(services, configuracao);

        var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetService<IServicoTelemetria>());
        Assert.NotNull(provider.GetService<IServicoLog>());
        Assert.NotNull(provider.GetService<IServicoMensageria>());
        Assert.NotNull(provider.GetService<IHttpClientFactory>());
    }
}
