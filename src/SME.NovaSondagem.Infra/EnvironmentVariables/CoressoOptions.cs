using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.EnvironmentVariables
{
    public class CoressoOptions
    {
        public static string Secao => "Coresso";
        public long SistemaId { get; set; }
        public long AcompanhamentoModuloId { get; set; }
    }
}