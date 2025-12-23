using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.NovaSondagem.Infra.Exceptions
{
    public class ValidacaoException : Exception
    {
        public readonly IEnumerable<ValidationFailure> Erros;

        public ValidacaoException(IEnumerable<ValidationFailure> erros)
        {
            this.Erros = erros;
        }

        public List<string> Mensagens() => Erros?.Select(c => c.ErrorMessage)?.ToList();
    }
}