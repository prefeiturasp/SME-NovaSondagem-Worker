namespace SME.NovaSondagem.Infra.EnvironmentVariables;

public class ConnectionStringOptions
{
    public const string Secao = "ConnectionStrings";
    public string SondagemConnection { get; set; } = string.Empty;
    public string SGP_PostgresConsultas { get; set; } = string.Empty;
    public string Eol_Postgres { get; set; } = string.Empty;
    public string Eol_SQLServer { get; set; } = string.Empty;
    public string CoreSSO { get; set; } = string.Empty;
}