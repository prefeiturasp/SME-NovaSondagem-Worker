using SME.NovaSondagem.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Interfaces
{
    public interface IServicoLog
    {
        void Registrar(Exception ex);
        void Registrar(string mensagem, Exception ex);
        void Registrar(LogNivel nivel, string erro, string observacoes, string stackTrace);
    }
}