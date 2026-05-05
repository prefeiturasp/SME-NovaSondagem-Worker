using Dapper;
using SME.NovaSondagem.Dados.Interfaces.Postgres;
using SME.NovaSondagem.Dominio.Entities.Postgres;
using SME.NovaSondagem.Infra.EnvironmentVariables;

namespace SME.NovaSondagem.Dados.Repositories.Postgres;

public class RepositorioGeneroSexo : RepositorioBase<GeneroSexo>, IRepositorioGeneroSexo
{
    public RepositorioGeneroSexo(ConnectionStringOptions connectionStrings) : base(connectionStrings)
    {
    }

    public async Task<bool> ExcluirAsync(long id)
    {
        const string query = "delete from genero_sexo where id = @Id;";

        using var conexao = ObterConexao();
        var linhasAfetadas = await conexao.ExecuteAsync(query, new { Id = id });
        return linhasAfetadas > 0;
    }
}
