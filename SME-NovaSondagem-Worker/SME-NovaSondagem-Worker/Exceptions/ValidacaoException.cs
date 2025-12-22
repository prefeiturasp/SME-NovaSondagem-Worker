using System;
using System.Collections.Generic;

namespace SME.NovaSondagem.Worker.Exceptions
{
    public class ValidacaoException : Exception
    {
        private readonly List<string> _mensagens;

        public ValidacaoException(List<string> mensagens) : base(string.Join(", ", mensagens))
        {
            _mensagens = mensagens;
        }

        public List<string> Mensagens() => _mensagens;
    }
}