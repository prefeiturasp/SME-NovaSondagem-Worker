namespace SME.NovaSondagem.Worker.Enums
{
    public static class ExchangeNovaSondagemRabbit
    {
        public const string NovaSondagem = "novasondagem";
        public const string NovaSondagemDeadLetter = "novasondagem.deadletter";
        public const string NovaSondagemLogs = "novasondagem.logs";
        public const string QueueLogs = "queue.logs";
        public const int NovaSondagemDeadLetterTTL = 30000; // 30 segundos
    }
}