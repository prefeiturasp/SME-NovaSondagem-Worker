using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SME.NovaSondagem.Worker.Contexts;
using SME.NovaSondagem.Worker.Options;
using SME.NovaSondagem.Worker.UseCases.ComponenteCurricular;
using SME.NovaSondagem.Workers.Worker;
using SME.NovaSondagem.Worker.Extensions;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var rabbitConfigs = builder.Configuration.AsEnumerable()
    .Where(c => !string.IsNullOrEmpty(c.Key) && c.Key.Contains("Rabbit", StringComparison.OrdinalIgnoreCase))
    .ToList();

foreach (var config in rabbitConfigs)
{
    var value = config.Key.Contains("Password", StringComparison.OrdinalIgnoreCase) ?
        (string.IsNullOrEmpty(config.Value) ? "NULL" : "***SET***") :
        (config.Value ?? "NULL");
    Console.WriteLine($"  {config.Key} = {value}");
}

var hostname = builder.Configuration["ConfiguracaoRabbitOptions:Hostname"];
var username = builder.Configuration["ConfiguracaoRabbitOptions:Username"];
var password = builder.Configuration["ConfiguracaoRabbitOptions:Password"];
var virtualhost = builder.Configuration["ConfiguracaoRabbitOptions:Virtualhost"];

if (string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
{
    var altHostname = builder.Configuration["ConfiguracaoRabbit:Hostname"];
    var altUsername = builder.Configuration["ConfiguracaoRabbit:Username"];
    var altPassword = builder.Configuration["ConfiguracaoRabbit:Password"];
    var altVirtualhost = builder.Configuration["ConfiguracaoRabbit:Virtualhost"];

    if (!string.IsNullOrEmpty(altHostname))
    {
        hostname = altHostname;
        username = altUsername;
        password = altPassword;
        virtualhost = altVirtualhost;
    }
    else
    {
        throw new InvalidOperationException("Configurações do RabbitMQ não foram encontradas. Verifique o arquivo secrets.json.");
    }
}
else
{
    Console.WriteLine("Configurações do RabbitMQ carregadas com sucesso!");
}

builder.Services.Configure<ConsumoFilasOptions>(builder.Configuration.GetSection(ConsumoFilasOptions.Secao));

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = hostname,
        Port = 5672,
        UserName = username,
        Password = password,
        VirtualHost = virtualhost ?? "/",
        RequestedHeartbeat = TimeSpan.FromSeconds(30),
        AutomaticRecoveryEnabled = true,
        NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
    };

    try
    {
        using var testConnection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        Console.WriteLine("Conexão RabbitMQ estabelecida com sucesso!");
        testConnection.CloseAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERRO na conexão RabbitMQ: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"   Detalhes: {ex.InnerException.Message}");
        }
    }

    return factory;
});

var eolConnection = builder.Configuration["EolConnection"];
if (!string.IsNullOrEmpty(eolConnection))
{
    builder.Services.AddDbContext<EolDbContext>(options =>
        options.UseNpgsql(eolConnection));
    Console.WriteLine("EolDbContext configurado");
}
else
{
    Console.WriteLine("EolConnection não encontrada");
}

var novaSondagemConnection = builder.Configuration["NovaSondagemConnection"];
if (!string.IsNullOrEmpty(novaSondagemConnection))
{
    builder.Services.AddDbContext<NovaSondagemDbContext>(options =>
        options.UseNpgsql(novaSondagemConnection));
    Console.WriteLine("NovaSondagemDbContext configurado");
}
else
{
    Console.WriteLine("NovaSondagemConnection não encontrada - comentando por enquanto");
}

builder.Services.AddScoped<BuscarComponenteCurricularUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRepositories();
builder.Services.AddUseCases();

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => new
{
    Status = "Running",
    Service = "SME Nova Sondagem Worker",
    Timestamp = DateTime.UtcNow
});

app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Service = "SME Nova Sondagem Worker",
    Timestamp = DateTime.UtcNow
}));

try
{
    Console.WriteLine("Iniciando aplicação na porta 5000...");
    Console.WriteLine("Acesse: http://localhost:5000");
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO FATAL: {ex.Message}");
    Console.WriteLine($"Tipo: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Causa: {ex.InnerException.Message}");
    }
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
}