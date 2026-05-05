using SME.NovaSondagem.Dominio.Entities.Postgres;

namespace SME.NovaSondagem.Dados.Interfaces.Postgres;

public interface IRepositorioGeneroSexo : IRepositorioBase<GeneroSexo>
{
    Task<bool> ExcluirAsync(long id);
}
