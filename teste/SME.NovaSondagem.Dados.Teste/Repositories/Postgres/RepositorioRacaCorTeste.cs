using SME.NovaSondagem.Dados.Interfaces.Postgres;
using SME.NovaSondagem.Dados.Repositories.Postgres;
using SME.NovaSondagem.Dominio.Entities.Postgres;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Dados.Teste.Repositories.Postgres;

public class RepositorioRacaCorTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_connection_string_for_nula()
    {
        var acao = () => new RepositorioRacaCor(null!);

        var ex = Assert.Throws<ArgumentNullException>(acao);
        Assert.Equal("connectionStrings", ex.ParamName);
    }

    [Fact]
    public void Deve_criar_repositorio_com_connection_string_valida()
    {
        var options = new ConnectionStringOptions
        {
            SondagemConnection = "Host=localhost;Database=sondagem;"
        };

        var repositorio = new RepositorioRacaCor(options);

        Assert.NotNull(repositorio);
    }

    [Fact]
    public void Deve_implementar_interface_do_repositorio_raca_cor()
    {
        var repositorio = new RepositorioRacaCor(new ConnectionStringOptions
        {
            SondagemConnection = "Host=localhost;Database=sondagem;"
        });

        Assert.IsAssignableFrom<IRepositorioRacaCor>(repositorio);
        Assert.IsAssignableFrom<SME.NovaSondagem.Dados.Interfaces.IRepositorioBase<RacaCor>>(repositorio);
    }
}
