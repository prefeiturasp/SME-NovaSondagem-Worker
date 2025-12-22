namespace SME.NovaSondagem.Worker.UseCases.ComponenteCurricular.Dtos
{
    public class BuscarComponenteCurricularRequest
    {
        public long? ComponenteCurricularId { get; set; }
        public bool ApenasAtivos { get; set; } = true;
        public string? UsuarioOperacao { get; set; }
    }
}