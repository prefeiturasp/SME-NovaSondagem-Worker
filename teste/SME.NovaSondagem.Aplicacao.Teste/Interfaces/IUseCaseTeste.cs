using SME.NovaSondagem.Aplicacao.Interfaces;
using SME.NovaSondagem.Infra.Fila;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Aplicacao.Teste.Interfaces;

public class UseCaseTeste
{
    [Fact]
    public async Task Deve_executar_contrato_do_use_case()
    {
        // arrange
        var casoDeUso = new UseCaseFake();
        var mensagem = new MensagemRabbit("{\"valor\":1}", Guid.NewGuid());

        // act
        var resultado = await casoDeUso.Executar(mensagem);

        // assert
        Assert.True(resultado);
    }

    private sealed class UseCaseFake : IUseCase
    {
        public Task<bool> Executar(MensagemRabbit mensagemRabbit)
            => Task.FromResult(mensagemRabbit != null);
    }
}
