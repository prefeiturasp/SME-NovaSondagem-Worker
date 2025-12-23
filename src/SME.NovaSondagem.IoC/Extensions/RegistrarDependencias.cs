using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Infra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.IoC.Extensions
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            services.AddPoliticas();

            RegistrarServicos(services);
            RegistrarRepositorios(services);
            RegistrarRepositoriosSerap(services);
            RegistrarRepositoriosCoresso(services);
            RegistrarRepositoriosEol(services);
            RegistrarCasosDeUso(services);
        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.TryAddSingleton<IServicoLog, ServicoLog>();
            services.TryAddSingleton<IServicoMensageria, ServicoMensageria>();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
           
        }

        private static void RegistrarRepositoriosEol(IServiceCollection services)
        {
            
        }

        private static void RegistrarRepositoriosCoresso(IServiceCollection services)
        {
            
        }

        private static void RegistrarRepositoriosSerap(IServiceCollection services)
        {
           
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
           
        }
    }
}