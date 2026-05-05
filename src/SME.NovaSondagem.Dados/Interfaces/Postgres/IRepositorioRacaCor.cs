using SME.NovaSondagem.Dominio.Entities.Postgres;

namespace SME.NovaSondagem.Dados.Interfaces.Postgres;

public interface IRepositorioRacaCor : IRepositorioBase<RacaCor>
{
    Task<bool> ExcluirAsync(long id);
}
