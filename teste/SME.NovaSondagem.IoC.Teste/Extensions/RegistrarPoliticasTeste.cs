using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Registry;
using SME.NovaSondagem.Infra.Policies;
using SME.NovaSondagem.IoC.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.IoC.Teste.Extensions;

public class RegistrarPoliticasTeste
{
    [Fact]
    public void Deve_registrar_politica_de_publicacao_no_registry()
    {
        var services = new ServiceCollection();

        services.AddPoliticas();
        var provider = services.BuildServiceProvider();
        var registry = provider.GetRequiredService<IPolicyRegistry<string>>();

        var policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);

        Assert.NotNull(policy);
    }
}
