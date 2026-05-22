using SME.NovaSondagem.Dados.Repositories;
using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Dados.Teste.Repositories;

public class RepositorioBaseTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_connection_string_for_nula()
    {
        // act
        var acao = () => new RepositorioFake(null!);

        // assert
        var excecao = Assert.Throws<ArgumentNullException>(acao);
        Assert.Equal("connectionStrings", excecao.ParamName);
    }

    private sealed class RepositorioFake : RepositorioBase<EntidadeFake>
    {
        public RepositorioFake(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }
    }

    private sealed class EntidadeFake : EntidadeBase
    {
    }
}
