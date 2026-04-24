using SME.NovaSondagem.Infra.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Extensions;

public class ExtensionMethodsTeste
{
    [Fact]
    public void Deve_obter_constantes_publicas_do_tipo()
    {
        var constantes = typeof(ConstantesTeste).ObterConstantesPublicas<string>();

        Assert.Single(constantes);
        Assert.Contains("valor.constante", constantes);
    }

    [Fact]
    public void Deve_obter_metodo_a_partir_da_interface()
    {
        var metodo = typeof(ImplementacaoTeste).ObterMetodo(nameof(IContratoTeste.ExecutarAsync));

        Assert.NotNull(metodo);
        Assert.Equal(nameof(IContratoTeste.ExecutarAsync), metodo.Name);
    }

    [Fact]
    public async Task Deve_invocar_metodo_assincrono_por_reflexao()
    {
        var alvo = new ImplementacaoTeste();
        var metodo = typeof(ImplementacaoTeste).GetMethod(nameof(ImplementacaoTeste.ExecutarAsync));

        var retorno = await metodo!.InvokeAsync(alvo, 3);

        Assert.Equal(6, (int)retorno);
    }

    private static class ConstantesTeste
    {
        public const string Valor = "valor.constante";
        public static string Ignorar => "propriedade";
    }

    private interface IContratoTeste
    {
        Task<int> ExecutarAsync(int valor);
    }

    private sealed class ImplementacaoTeste : IContratoTeste
    {
        public async Task<int> ExecutarAsync(int valor)
        {
            await Task.Yield();
            return valor * 2;
        }
    }
}
