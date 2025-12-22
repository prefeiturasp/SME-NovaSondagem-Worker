using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SME.NovaSondagem.Worker.Dtos.Eol
{
    [Table("componente_curricular")]
    public class ComponenteCurricularEol
    {
        [Key]
        [Column("cd_componente_curricular")]
        public long CdComponenteCurricular { get; set; }

        [Column("dc_componente_curricular")]
        [MaxLength(255)]
        public string DcComponenteCurricular { get; set; } = string.Empty;

        [Column("dt_atualizacao_tabela")]
        public DateTime? DtAtualizacaoTabela { get; set; }

        [Column("cd_operador")]
        [MaxLength(50)]
        public string? CdOperador { get; set; }

        [Column("in_polivalencia")]
        public bool InPolivalencia { get; set; }

        [Column("in_programa_disciplina")]
        public bool InProgramaDisciplina { get; set; }

        [Column("dt_cancelamento")]
        public DateTime? DtCancelamento { get; set; }

        [Column("cd_disciplina_mec")]
        public int? CdDisciplinaMec { get; set; }

        [Column("cd_componente_curricular_principal")]
        public long? CdComponenteCurricularPrincipal { get; set; }

        [Column("cd_componente_equivalente_quadro")]
        public int? CdComponenteEquivalenteQuadro { get; set; }

        [Column("in_sp_integral")]
        public bool InSpIntegral { get; set; }
    }
}