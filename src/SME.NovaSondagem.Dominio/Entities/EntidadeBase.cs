namespace SME.NovaSondagem.Dominio.Entities;

public abstract class EntidadeBase
{
    public long Id { get; set; }
    public DateTime? AlteradoEm { get; set; }
    public string? AlteradoPor { get; set; } = string.Empty;
    public string? AlteradoRF { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
    public string CriadoPor { get; set; } = string.Empty;
    public string CriadoRF { get; set; } = string.Empty;
    public bool Excluido { get; set; } = false;
}