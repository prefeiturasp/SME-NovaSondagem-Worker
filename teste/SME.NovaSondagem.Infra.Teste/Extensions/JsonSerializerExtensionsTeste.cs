using SME.NovaSondagem.Infra.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Extensions;

public class JsonSerializerExtensionsTeste
{
    [Fact]
    public void Deve_serializar_e_desserializar_objeto()
    {
        // arrange
        var origem = new DtoTeste { Nome = "NovaSondagem", Valor = 10 };

        // act
        var json = origem.ConverterObjectParaJson();
        var destino = json.ConverterObjectStringPraObjeto<DtoTeste>();

        // assert
        Assert.False(string.IsNullOrWhiteSpace(json));
        Assert.NotNull(destino);
        Assert.Equal("NovaSondagem", destino.Nome);
        Assert.Equal(10, destino.Valor);
    }

    private sealed class DtoTeste
    {
        public string Nome { get; set; } = string.Empty;
        public int Valor { get; set; }
    }
}
