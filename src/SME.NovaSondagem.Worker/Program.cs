using Elastic.Apm.AspNetCore;
using Elastic.Apm.Config;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.Instrumentations.SqlClient;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SME.NovaSondagem.Dados.Interceptors;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Infra.Services;
using SME.NovaSondagem.Worker;
using SME.NovaSondagem.Dados;
using SME.NovaSondagem.Infra;
using SME.NovaSondagem.IoC;
using System;
using System.Threading;
using SME.NovaSondagem.IoC.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using System.Reflection;

namespace SME.NovaSondagem.Worker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(a => a.AddUserSecrets(Assembly.GetExecutingAssembly()))
           .UseStartup<Startup>();
    }
}