using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Dominio.Enums;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Dominio.Teste.Entities;

public class LogMensagemTeste
{
    [Fact]
    public void Deve_criar_log_mensagem_com_propriedades_informadas()
    {
        // arrange
        var antes = DateTime.Now;

        // act
        var log = new LogMensagem(
            mensagem: "falha no processamento",
            nivel: LogNivel.Critico,
            observacao: "erro ao consumir fila",
            rastreamento: "stacktrace",
            excecaoInterna: "inner");

        var depois = DateTime.Now;

        // assert
        Assert.Equal("falha no processamento", log.Mensagem);
        Assert.Equal(LogNivel.Critico, log.Nivel);
        Assert.Equal("erro ao consumir fila", log.Observacao);
        Assert.Equal("stacktrace", log.Rastreamento);
        Assert.Equal("inner", log.ExcecaoInterna);
        Assert.Equal("SME.NovaSondagem.Worker", log.Projeto);
        Assert.True(log.DataHora >= antes && log.DataHora <= depois);
    }
}
