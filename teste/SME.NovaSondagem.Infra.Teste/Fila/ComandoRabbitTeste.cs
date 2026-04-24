using SME.NovaSondagem.Infra.Fila;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Fila;

public class ComandoRabbitTeste
{
    [Fact]
    public void Deve_criar_comando_com_valores_padrao()
    {
        var comando = new ComandoRabbit("Processar", typeof(string));

        Assert.Equal("Processar", comando.NomeProcesso);
        Assert.Equal(typeof(string), comando.TipoCasoUso);
        Assert.Equal((ulong)3, comando.QuantidadeReprocessamentoDeadLetter);
        Assert.Equal(10 * 60 * 100, comando.Ttl);
        Assert.False(comando.ModeLazy);
    }

    [Fact]
    public void Deve_criar_comando_lazy_com_todos_os_campos()
    {
        var comando = new ComandoRabbit("Processar", typeof(int), true, 5, 5000);

        Assert.True(comando.ModeLazy);
        Assert.Equal((ulong)5, comando.QuantidadeReprocessamentoDeadLetter);
        Assert.Equal(5000, comando.Ttl);
    }
}
