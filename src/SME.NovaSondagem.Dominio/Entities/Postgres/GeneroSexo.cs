namespace SME.NovaSondagem.Dominio.Entities.Postgres;

public class GeneroSexo : EntidadeBase
{
    public required string Descricao { get; set; }
    public string? Sigla { get; set; }
}