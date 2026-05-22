using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.IoC;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.IoC.Teste;

public class ConfigureServicesTeste
{
    [Fact]
    public void Deve_configurar_connection_string_options_no_container()
    {
        // arrange
        var configuracao = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SondagemConnection"] = "Host=localhost;Database=sondagem;",
                ["ConnectionStrings:SGP_PostgresConsultas"] = "Host=localhost;Database=sgp;",
                ["ConnectionStrings:Eol_Postgres"] = "Host=localhost;Database=eol;",
                ["ConnectionStrings:Eol_SQLServer"] = "Server=localhost;Database=eol;",
                ["ConnectionStrings:CoreSSO"] = "Server=localhost;Database=coresso;"
            })
            .Build();

        var services = new ServiceCollection();

        // act
        ConfigureServices.ConfigurarConexoes(services, configuracao);
        var provider = services.BuildServiceProvider();
        var options = provider.GetService<ConnectionStringOptions>();
        var safeOptions = Assert.IsType<ConnectionStringOptions>(options);

        // assert
        Assert.Equal("Host=localhost;Database=sondagem;", safeOptions.SondagemConnection);
        Assert.Equal("Host=localhost;Database=sgp;", safeOptions.SGP_PostgresConsultas);
    }
}
