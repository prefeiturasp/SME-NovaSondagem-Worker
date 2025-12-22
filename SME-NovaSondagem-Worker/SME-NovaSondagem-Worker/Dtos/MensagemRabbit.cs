using Newtonsoft.Json;
using System;

namespace SME.NovaSondagem.Worker.Dtos
{
    public class MensagemRabbit
    {
        public Guid Id { get; set; }
        public object Mensagem { get; set; }
        public string UsuarioLogadoRF { get; set; }
        public bool NotificarErroUsuario { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;

        internal T ObterObjetoMensagem<T>()
        {
            return JsonConvert.DeserializeObject<T>(Mensagem.ToString());
        }
    }
}