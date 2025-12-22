using Microsoft.EntityFrameworkCore;
using SME.NovaSondagem.Worker.Contexts;
using SME.NovaSondagem.Worker.Dtos;
using SME.NovaSondagem.Worker.Dtos.Eol;
using SME.NovaSondagem.Worker.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Repositories
{
    public class ComponenteCurricularRepository : RepositoryBase<ComponenteCurricular>, IComponenteCurricularRepository
    {
        private readonly EolDbContext _eolContext;
        private int _contadorParaLote = 0;

        public ComponenteCurricularRepository(
            NovaSondagemDbContext context,
            EolDbContext eolContext) : base(context)
        {
            _eolContext = eolContext;
        }

        public async Task<ComponenteCurricular?> ObterPorCodigoEolAsync(long codigoEol)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CodigoEol == codigoEol);
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesEolAtivosAsync()
        {
            return await _eolContext.ComponentesCurriculares
                .Where(c => c.DtCancelamento == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SalvarEmLoteAsync(int tamanhoLote = 100)
        {
            _contadorParaLote++;

            if (_contadorParaLote % tamanhoLote == 0)
            {
                await _eolContext.SaveChangesAsync();
                _contadorParaLote = 0;
            }
        }
    }
}