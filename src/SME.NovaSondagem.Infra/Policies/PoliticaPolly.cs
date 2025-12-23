using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Policies
{
    public class PoliticaPolly
    {
        public static string PublicaFila => "RetryPolicyFilasRabbit";
    }
}