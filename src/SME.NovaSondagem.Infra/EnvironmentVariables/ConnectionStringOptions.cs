using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.EnvironmentVariables
{
    public class ConnectionStringOptions
    {
        public static string Secao => "ConnectionStrings";
        public string SondagemConnection { get; set; }
        public string SGP_PostgresConsultas { get; set; }
        public string Eol_Postgres { get; set; }
        public string Eol_SQLServer { get; set; }
        public string CoreSSO { get; set; }
    }
}