using Dommel;
using SME.NovaSondagem.Dados.Interfaces;
using Npgsql;
using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using System.Data;

namespace SME.NovaSondagem.Dados.Repositories;

public abstract class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
{
    private readonly ConnectionStringOptions connectionStrings;

    protected RepositorioBase(ConnectionStringOptions connectionStrings)
    {
        this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
    }

    protected IDbConnection ObterConexao()
    {
        var conexao = new NpgsqlConnection(connectionStrings.SondagemConnection);
        conexao.Open();
        return conexao;
    }

    public virtual async Task<IEnumerable<T>> ObterTudoAsync()
    {
        using var conexao = ObterConexao();
        try
        {
            return await conexao.GetAllAsync<T>();
        }
        finally
        {
            conexao.Close();
            conexao.Dispose();
        }
    }

    public virtual async Task<T> ObterPorIdAsync(long id)
    {
        using var conexao = ObterConexao();
        try
        {
            return await conexao.GetAsync<T>(id: id);
        }
        finally
        {
            conexao.Close();
            conexao.Dispose();
        }
    }

    public virtual async Task<long> SalvarAsync(T entidade)
    {
        var conexao = ObterConexao();
        try
        {
            if (entidade.Id > 0)
            {
                await conexao.UpdateAsync(entidade);
            }
            else
            {
                entidade.Id = (long)await conexao.InsertAsync(entidade);
            }
            return entidade.Id;
        }
        finally
        {
            conexao.Close();
            conexao.Dispose();
        }
    }

    public virtual async Task<long> UpdateAsync(T entidade)
    {
        var conexao = ObterConexao();
        try
        {
            await conexao.UpdateAsync(entidade);

            return entidade.Id;
        }
        finally
        {
            conexao.Close();
            conexao.Dispose();
        }
    }

    public virtual async Task<long> IncluirAsync(T entidade)
    {
        var conexao = ObterConexao();
        try
        {
            entidade.Id = (long)await conexao.InsertAsync(entidade);
            return entidade.Id;
        }
        finally
        {
            conexao.Close();
            conexao.Dispose();
        }
    }
}