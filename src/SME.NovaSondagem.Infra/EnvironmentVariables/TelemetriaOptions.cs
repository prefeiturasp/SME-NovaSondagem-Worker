namespace SME.NovaSondagem.Infra.EnvironmentVariables;

public class TelemetriaOptions
{
    public const string Secao = "Telemetria";
    public bool ApplicationInsights { get; set; }
    public bool Apm { get; set; }
}