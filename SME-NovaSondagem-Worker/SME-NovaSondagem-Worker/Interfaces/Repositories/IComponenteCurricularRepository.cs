using SME.NovaSondagem.Worker.Dtos;
using SME.NovaSondagem.Worker.Dtos.Eol;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Interfaces.Repositories
{
    public interface IComponenteCurricularRepository : IRepositoryBase<ComponenteCurricular>
    {
        Task<ComponenteCurricular?> ObterPorCodigoEolAsync(long codigoEol);
        Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesEolAtivosAsync();
    }
}