using Microsoft.Extensions.DependencyInjection;
using SME.NovaSondagem.Worker.Interfaces.Repositories;
using SME.NovaSondagem.Worker.Interfaces.UseCases;
using SME.NovaSondagem.Worker.Repositories;
using SME.NovaSondagem.Worker.UseCases.ComponenteCurricular;

namespace SME.NovaSondagem.Worker.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IComponenteCurricularRepository, ComponenteCurricularRepository>();

            return services;
        }

        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IBuscarComponenteCurricularUseCase, BuscarComponenteCurricularUseCase>();

            return services;
        }
    }
}