namespace SME.NovaSondagem.Dominio.Entities.Postgres;

public class RacaCor : EntidadeBase
{
    public required string Descricao { get; set; }
    public required int CodigoEolRacaCor { get; set; }
}
