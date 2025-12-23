using Elasticsearch.Net;
using Nest;
using SME.NovaSondagem.Dados.Interfaces.Elastic;
using SME.NovaSondagem.Dominio.Entities.Elastic;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.NovaSondagem.Dados.Repositories.Elastic
{
    public abstract class RepositorioElasticBase<TEntidade> : IRepositorioElasticBase<TEntidade> where TEntidade : EntidadeBaseElastic
    {
        protected readonly IElasticClient elasticClient;
        protected readonly string IndexName;

        public RepositorioElasticBase(ElasticOptions elasticOptions, IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
            if (elasticOptions == null) throw new ArgumentNullException(nameof(elasticOptions));

            if (!string.IsNullOrEmpty(elasticOptions.PrefixIndex))
                IndexName = $"{elasticOptions.PrefixIndex}-";

            IndexName = (IndexName + typeof(TEntidade).Name).ToLower();

            _ = CriarIndexAsync().Result;
        }

        public virtual async Task<bool> CriarIndexAsync()
        {
            if (!(await elasticClient.Indices.ExistsAsync(IndexName)).Exists)
            {
                await elasticClient.Indices.CreateAsync(IndexName, c =>
                {
                    c.Map<TEntidade>(p => p.AutoMap());
                    return c;
                });
            }
            return true;
        }

        public virtual async Task<bool> InserirAsync(TEntidade entidade)
        {
            var response = await elasticClient.IndexAsync(entidade, descriptor => descriptor.Index(IndexName));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public virtual async Task<bool> AlterarAsync(TEntidade entidade)
        {
            var response = await elasticClient.UpdateAsync(
                DocumentPath<TEntidade>
                    .Id(entidade.Id)
                    .Index(IndexName), p => p.Doc(entidade)
                        .RetryOnConflict(3)
                        .Refresh(Refresh.True));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public virtual async Task<bool> DeletarAsync(string id)
        {
            var response = await elasticClient.DeleteAsync(DocumentPath<TEntidade>.Id(id).Index(IndexName));

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public async Task<TEntidade> ObterPorIdAsync(string id)
        {
            var response = await elasticClient.GetAsync(DocumentPath<TEntidade>.Id(id).Index(IndexName));

            if (!response.IsValid) return default;

            return response.Source;
        }

        public async Task<IEnumerable<TEntidade>> ObterTodosAsync()
        {
            var search = new SearchDescriptor<TEntidade>(IndexName)
                .Size(10000)
                .Scroll("1m");

            var response = await elasticClient.SearchAsync<TEntidade>(search);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            var retorno = new List<TEntidade>();

            while (response.Hits.Any())
            {
                retorno.AddRange(response.Hits.Select(hit => hit.Source).ToList());
                response = await elasticClient.ScrollAsync<TEntidade>("1m", response.ScrollId);
            }

            return retorno;
        }
    }
}