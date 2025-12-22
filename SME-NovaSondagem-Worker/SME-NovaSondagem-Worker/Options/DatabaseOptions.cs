namespace SME.NovaSondagem.Worker.Options
{
    public class DatabaseOptions
    {
        public const string Secao = "Database";
        public string ConnectionStringEol { get; set; } = string.Empty;
        public string ConnectionStringNovaSondagem { get; set; } = string.Empty;
    }
}