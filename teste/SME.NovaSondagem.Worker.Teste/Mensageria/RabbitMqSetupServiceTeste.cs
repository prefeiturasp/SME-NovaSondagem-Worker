using Moq;
using RabbitMQ.Client;
using SME.NovaSondagem.Infra.EnvironmentVariables;
using SME.NovaSondagem.Infra.Fila;
using SME.NovaSondagem.Worker.Mensageria;
using Xunit;
using Assert = Xunit.Assert;

namespace SME.NovaSondagem.Worker.Teste.Mensageria;

public class RabbitMqSetupServiceTeste
{
    [Fact]
    public async Task Deve_configurar_exchange_e_filas_no_channel()
    {
        var rabbitOptions = new RabbitOptions
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            LimiteDeMensagensPorExecucao = 5,
            ForcarRecriarFilas = false
        };

        var service = new RabbitMqSetupService(rabbitOptions);
        var channel = new Mock<IChannel>();
        var comandos = new Dictionary<string, ComandoRabbit>
        {
            [RotasRabbit.IniciarSync] = new ComandoRabbit("Processar", typeof(string), true, 4, 1234)
        };

        await service.SetupExchangesAndQueuesAsync(channel.Object, comandos);

        channel.Verify(c => c.BasicQosAsync(0, 5, false, It.IsAny<CancellationToken>()), Times.Once);
        channel.Verify(c => c.ExchangeDeclareAsync(ExchangeRabbit.NovaSondagem, ExchangeType.Direct, true, false, It.IsAny<IDictionary<string, object?>>(), false, false, It.IsAny<CancellationToken>()), Times.Once);
        channel.Verify(c => c.QueueDeclareAsync(RotasRabbit.IniciarSync, true, false, false, It.IsAny<IDictionary<string, object?>>(), false, false, It.IsAny<CancellationToken>()), Times.Once);
        channel.Verify(c => c.QueueBindAsync(RotasRabbit.IniciarSync, ExchangeRabbit.NovaSondagem, RotasRabbit.IniciarSync, It.IsAny<IDictionary<string, object?>>(), false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Deve_lancar_excecao_quando_rabbit_options_for_nulo()
    {
        var acao = () => new RabbitMqSetupService(null!);

        var ex = Assert.Throws<ArgumentNullException>(acao);

        Assert.Equal("rabbitOptions", ex.ParamName);
    }
}
