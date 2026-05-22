using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Exceptions
{
    public class ErroException : Exception
    {
        public ErroException(string mensagem, int statusCode = 500) : base(mensagem)
        {
            StatusCode = statusCode;
        }

        public ErroException(string mensagem, HttpStatusCode statusCode) : base(mensagem)
        {
            StatusCode = (int)statusCode;
        }

        public int StatusCode { get; }
    }
}