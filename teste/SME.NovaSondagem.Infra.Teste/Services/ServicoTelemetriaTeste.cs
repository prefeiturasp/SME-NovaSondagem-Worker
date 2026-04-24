using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Infra.Teste.Services;

public class ServicoTelemetriaTeste
{
    [Fact]
    public void Deve_iniciar_transacao_com_temporizador_quando_appinsights_ativo()
    {
        var servico = new ServicoTelemetria(new TelemetriaOptions { ApplicationInsights = true });

        var transacao = servico.IniciarTransacao("rota.teste");

        Assert.Equal("rota.teste", transacao.Nome);
        Assert.NotEqual(default, transacao.InicioOperacao);
        Assert.NotNull(transacao.Temporizador);
    }

    [Fact]
    public async Task Deve_registrar_async_sem_lancar_excecao()
    {
        var servico = new ServicoTelemetria(new TelemetriaOptions { ApplicationInsights = false });
        var executou = false;

        await servico.RegistrarAsync(async () =>
        {
            executou = true;
            await Task.CompletedTask;
        }, "acao", "tag", "valor");

        Assert.True(executou);
    }

    [Fact]
    public async Task Deve_registrar_com_retorno_async()
    {
        var servico = new ServicoTelemetria(new TelemetriaOptions());

        var resultado = await servico.RegistrarComRetornoAsync<object>(
            () => Task.FromResult<object>("ok"),
            "acao",
            "tag",
            "valor");

        Assert.Equal("ok", (string)resultado);
    }
}
