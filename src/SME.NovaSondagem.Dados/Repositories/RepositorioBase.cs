using Dommel;
using Npgsql;
using SME.NovaSondagem.Dominio.Entities;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Dados.Repositories
{
    public abstract class RepositorioBase<T> where T : EntidadeBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioBase(ConnectionStringOptions connectionStrings)
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

        protected IDbConnection ObterConexaoSgp()
        {
            var conexao = new NpgsqlConnection(connectionStrings.SGP_PostgresConsultas);
            conexao.Open();
            return conexao;
        }

        protected IDbConnection ObterConexaoEolPostgres()
        {
            var conexao = new NpgsqlConnection(connectionStrings.Eol_Postgres);
            conexao.Open();
            return conexao;
        }

        protected virtual IDbConnection ObterConexaoEolSqlServer()
        {
            var conexao = new SqlConnection(connectionStrings.Eol_SQLServer);
            conexao.Open();
            return conexao;
        }

        protected virtual IDbConnection ObterConexaoCoreSSO()
        {
            var conexao = new SqlConnection(connectionStrings.CoreSSO);
            conexao.Open();
            return conexao;
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
                    conexao.Update(entidade);
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
}