using SME.NovaSondagem.Infra.Fila;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Fila;

public class MensagemRabbitTeste
{
    [Fact]
    public void Deve_obter_objeto_tipado_da_mensagem_json()
    {
        var codigo = Guid.NewGuid();
        var mensagem = new MensagemRabbit("{\"Nome\":\"Item\",\"Valor\":7}", codigo);

        var objeto = mensagem.ObterObjetoMensagem<ObjetoTeste>();

        Assert.NotNull(objeto);
        Assert.Equal("Item", objeto.Nome);
        Assert.Equal(7, objeto.Valor);
        Assert.Equal(codigo, mensagem.CodigoCorrelacao);
    }

    [Fact]
    public void Deve_retornar_nulo_quando_mensagem_for_nula()
    {
        var mensagem = new MensagemRabbit(null!, Guid.NewGuid());

        var objeto = mensagem.ObterObjetoMensagem<ObjetoTeste>();

        Assert.Null(objeto);
    }

    private sealed class ObjetoTeste
    {
        public string Nome { get; set; } = string.Empty;
        public int Valor { get; set; }
    }
}
