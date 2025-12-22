using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.NovaSondagem.Worker.Options;
using SME.NovaSondagem.Worker.Rotas;
using SME.NovaSondagem.Worker.UseCases.ComponenteCurricular;
using SME.NovaSondagem.Worker.Workers.Worker;

namespace SME.NovaSondagem.Workers.Worker
{
    public class Worker : WorkerRabbitSondagem
    {
        public Worker(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory connectionFactory,
            ILogger<Worker> logger)
            : base(serviceScopeFactory, consumoFilasOptions, connectionFactory, "Nova Sondagem Worker", typeof(RotasApiEol), logger)
        {
        }

        protected override void RegistrarUseCases()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var buscarComponenteCurricularUseCase = scope.ServiceProvider.GetRequiredService<BuscarComponenteCurricularUseCase>();
            
            Comandos.TryAdd(RotasApiEol.RotaBuscarComponenteCurricularEol, buscarComponenteCurricularUseCase);
            
            _logger.LogInformation("Registrados {Count} Use Cases:", Comandos.Count);
            foreach (var comando in Comandos.Keys)
            {
                _logger.LogInformation("  - {Rota}", comando);
            }
        }
    }
}