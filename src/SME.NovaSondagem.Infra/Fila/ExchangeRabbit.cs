namespace SME.NovaSondagem.Infra.Fila;

public static class ExchangeRabbit
{
    public static string NovaSondagem => "novasondagem.workers";
    public static string NovaSondagemDeadLetter => "novasondagem.workers.deadletter";
    public static string Logs => "EnterpriseApplicationLog";
    public static int NovaSondagemDeadLetterTtl => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
    public static int NovaSondagemDeadLetterTtl_3 => 3 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
}