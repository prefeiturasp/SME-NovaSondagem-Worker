using SME.NovaSondagem.Infra.Fila;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Aplicacao.Interfaces
{
    public interface IUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}