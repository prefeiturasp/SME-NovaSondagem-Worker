using SME.NovaSondagem.Dominio.Entities.Elastic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.NovaSondagem.Dados.Interfaces.Elastic
{
    public interface IRepositorioElasticBase<TEntidade> where TEntidade : EntidadeBaseElastic
    {
        Task<bool> AlterarAsync(TEntidade entidade);
        Task<bool> CriarIndexAsync();
        Task<bool> DeletarAsync(string id);
        Task<bool> InserirAsync(TEntidade entidade);
        Task<TEntidade> ObterPorIdAsync(string id);
        Task<IEnumerable<TEntidade>> ObterTodosAsync();
    }
}