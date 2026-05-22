namespace SME.NovaSondagem.Infra.Fila;

public static class RotasRabbit
{
    public static string RotaLogs => "ApplicationLog";
    public static string Log => "ApplicationLog";
    public const string IniciarSync = "novasondagem.iniciar.sync";
}