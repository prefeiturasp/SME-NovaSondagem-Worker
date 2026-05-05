using SME.NovaSondagem.Dados.Interfaces.Postgres;
using SME.NovaSondagem.Dados.Repositories.Postgres;
using SME.NovaSondagem.Dominio.Entities.Postgres;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Dados.Teste.Repositories.Postgres;

public class RepositorioGeneroSexoTeste
{
    [Fact]
    public void Deve_lancar_excecao_quando_connection_string_for_nula()
    {
        var acao = () => new RepositorioGeneroSexo(null!);

        var ex = Assert.Throws<ArgumentNullException>(acao);
        Assert.Equal("connectionStrings", ex.ParamName);
    }

    [Fact]
    public void Deve_implementar_interface_do_repositorio_genero_sexo()
    {
        var repositorio = new RepositorioGeneroSexo(new ConnectionStringOptions
        {
            SondagemConnection = "Host=localhost;Database=sondagem;"
        });

        Assert.IsAssignableFrom<IRepositorioGeneroSexo>(repositorio);
        Assert.IsAssignableFrom<SME.NovaSondagem.Dados.Interfaces.IRepositorioBase<GeneroSexo>>(repositorio);
    }
}
