using Microsoft.EntityFrameworkCore;
using SME.NovaSondagem.Worker.Contexts;
using SME.NovaSondagem.Worker.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly NovaSondagemDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(NovaSondagemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> ObterPorIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task InserirAsync(TEntity entidade)
        {
            await _dbSet.AddAsync(entidade);
        }

        public virtual async Task InserirEmMassaAsync(IEnumerable<TEntity> entidades)
        {
            await _dbSet.AddRangeAsync(entidades);
        }

        public virtual Task AtualizarAsync(TEntity entidade)
        {
            _dbSet.Update(entidade);
            return Task.CompletedTask;
        }

        public virtual async Task DeletarAsync(long id)
        {
            var entidade = await ObterPorIdAsync(id);
            if (entidade != null)
            {
                _dbSet.Remove(entidade);
            }
        }

        public virtual async Task<int> SalvarAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}