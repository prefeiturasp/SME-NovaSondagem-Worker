using SME.NovaSondagem.Worker.Dtos;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Worker.Interfaces.Repositories
{
    public interface IRabbitUseCase
    {
        Task Executar(MensagemRabbit mensagem);
    }
}