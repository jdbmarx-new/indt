using Common.Domain;

namespace PropostaApi.Domain.Entities;

public class Proposta : Entity
{
    public string Cliente { get; set; } = string.Empty;
    public string Produto { get; set; } = string.Empty;
    public PropostaStatus Status { get; set; } = PropostaStatus.EmAnalise;
    public DateTime CriadaEm { get; set; } = DateTime.UtcNow;
}