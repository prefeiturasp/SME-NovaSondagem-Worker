using SME.NovaSondagem.Infra.Fila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Interfaces
{
    public interface IServicoMensageria
    {
        Task<bool> Publicar(MensagemRabbit mensagemRabbit, string rota, string exchange, string nomeAcao);
    }
}