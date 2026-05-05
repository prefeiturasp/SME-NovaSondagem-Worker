using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SME.NovaSondagem.Dados.Interfaces.Postgres;
using SME.NovaSondagem.Dados.Repositories.Postgres;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Interfaces;
using SME.NovaSondagem.Infra.Services;

namespace SME.NovaSondagem.IoC.Extensions;

public static class RegistraDependencias
{
    public static void Registrar(IServiceCollection services, IConfiguration configuration)
    {
        ConfigurarRabbitmq(services, configuration);
        ConfigurarRabbitmqLog(services, configuration);
        ConfigurarTelemetria(services, configuration);

        services.AdicionarValidadoresFluentValidation();
        services.AddPoliticas();

        RegistrarServicos(services, configuration);
    }

    private static void RegistrarServicos(IServiceCollection services, IConfiguration configuration)
    {
        var telemetria = new TelemetriaOptions();
        configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetria, c => c.BindNonPublicProperties = true);
        services.AddSingleton(telemetria);

        services.AddSingleton(provider => provider.GetRequiredService<IOptions<TelemetriaOptions>>().Value);

        services.TryAddScoped<IServicoTelemetria, ServicoTelemetria>();
        services.TryAddScoped<IServicoLog, ServicoLog>();
        services.TryAddSingleton<IServicoMensageria, ServicoMensageria>();
        services.TryAddScoped<IRepositorioRacaCor, RepositorioRacaCor>();
        services.TryAddScoped<IRepositorioGeneroSexo, RepositorioGeneroSexo>();
        services.AddHttpClient();
        services.AdicionarHttpClients(configuration);
    }

    private static void ConfigurarRabbitmq(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitOptions = new RabbitOptions();
        configuration.GetSection(RabbitOptions.Secao).Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
        services.AddSingleton(rabbitOptions);
    }

    private static void ConfigurarRabbitmqLog(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitLogOptions = new RabbitLogOptions();
        configuration.GetSection(RabbitLogOptions.Secao).Bind(rabbitLogOptions, c => c.BindNonPublicProperties = true);
        services.AddSingleton(rabbitLogOptions);
    }

    private static void ConfigurarTelemetria(IServiceCollection services, IConfiguration configuration)
    {
        var telemetriaOptions = new TelemetriaOptions();
        configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);
        services.AddSingleton(telemetriaOptions);

        var servicoTelemetria = new ServicoTelemetria(telemetriaOptions);
        services.AddSingleton(servicoTelemetria);
    }
}