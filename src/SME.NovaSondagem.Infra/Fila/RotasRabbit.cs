using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Fila
{
    public class RotasRabbit
    {
        public static string RotaLogs => "ApplicationLog";
        public static string Log => "ApplicationLog";
        public const string IniciarSync = "novasondagem.iniciar.sync";
    }
}