using SME.NovaSondagem.Dados.Repositories;
using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using System.Reflection;
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

    [Fact]
    public void Deve_implementar_contrato_i_repositorio_base()
    {
        var repositorio = new RepositorioFake(new ConnectionStringOptions
        {
            SondagemConnection = "Host=localhost;Database=sondagem;"
        });

        Assert.IsAssignableFrom<SME.NovaSondagem.Dados.Interfaces.IRepositorioBase<EntidadeFake>>(repositorio);
    }

    [Fact]
    public void Deve_conter_metodos_crud_no_repositorio_base()
    {
        var tipo = typeof(RepositorioBase<EntidadeFake>);

        Assert.NotNull(tipo.GetMethod("SalvarAsync", BindingFlags.Instance | BindingFlags.Public));
        Assert.NotNull(tipo.GetMethod("ObterPorIdAsync", BindingFlags.Instance | BindingFlags.Public));
        Assert.NotNull(tipo.GetMethod("IncluirAsync", BindingFlags.Instance | BindingFlags.Public));
        Assert.NotNull(tipo.GetMethod("UpdateAsync", BindingFlags.Instance | BindingFlags.Public));
        Assert.NotNull(tipo.GetMethod("ObterTudoAsync", BindingFlags.Instance | BindingFlags.Public));
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
