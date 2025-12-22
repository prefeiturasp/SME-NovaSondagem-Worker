using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity?> ObterPorIdAsync(long id);
        Task<IEnumerable<TEntity>> ObterTodosAsync();
        Task InserirAsync(TEntity entidade);
        Task InserirEmMassaAsync(IEnumerable<TEntity> entidades);
        Task AtualizarAsync(TEntity entidade);
        Task DeletarAsync(long id);
        Task<int> SalvarAsync();
    }
}
