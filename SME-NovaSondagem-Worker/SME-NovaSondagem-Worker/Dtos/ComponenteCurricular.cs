using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SME.NovaSondagem.Worker.Dtos
{
    [Table("componente_curricular")]
    public class ComponenteCurricular
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long CodigoEol { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descricao { get; set; } = string.Empty;

        public bool Polivalencia { get; set; }

        public bool ProgramaDisciplina { get; set; }

        public int? CodigoDisciplinaMec { get; set; }

        public long? CodigoComponentePrincipal { get; set; }

        public int? CodigoComponenteEquivalenteQuadro { get; set; }

        public bool SpIntegral { get; set; }

        public bool Ativo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; set; }

        [MaxLength(50)]
        public string? CriadoPor { get; set; }

        [MaxLength(50)]
        public string? AlteradoPor { get; set; }
    }
}
