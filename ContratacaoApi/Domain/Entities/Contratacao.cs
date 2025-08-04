using Common.Domain;

namespace ContratacaoApi.Domain.Entities
{
    public class Contratacao : Entity
    {
        public int PropostaId { get; set; }
        public DateTime DataContratacao { get; set; }
    }
}