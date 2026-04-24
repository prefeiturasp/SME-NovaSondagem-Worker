namespace SME.NovaSondagem.Infra.Policies;

public static class PoliticaPolly
{
    public static string PublicaFila => "RetryPolicyFilasRabbit";
}