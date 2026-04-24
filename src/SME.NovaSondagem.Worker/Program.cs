using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.IoC;
using SME.NovaSondagem.IoC.Extensions;
using SME.NovaSondagem.Worker.Mensageria;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Serilog;
using Elastic.Apm.SerilogEnricher;

[assembly: ExcludeFromCodeCoverage]
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithElasticApmCorrelationInfo()
    .WriteTo.Console()
    .CreateLogger();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);

ConfigureServices.ConfigurarConexoes(builder.Services, builder.Configuration);
RegistraDependencias.Registrar(builder.Services, builder.Configuration);

builder.Services.AddAllElasticApm();

builder.Services.AddSingleton<IRabbitMqSetupService, RabbitMqSetupService>();
builder.Services.AddSingleton<IRabbitMqMessageProcessor, RabbitMqMessageProcessor>();

ConfigureServices.ConfigurarServicos(builder.Services, builder.Configuration);

builder.Services.AddHostedService<RabbitMqConsumerService>();

IHost host = builder.Build();
await host.RunAsync();