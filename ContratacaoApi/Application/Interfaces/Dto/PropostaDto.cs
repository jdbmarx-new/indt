namespace ContratacaoApi.Application.Interfaces.Dto;

public class PropostaDto
{
    public int Id { get; set; }
    public string Cliente { get; set; }
    public string Produto { get; set; }
    public PropostaStatus Status { get; set; }
    public DateTime CriadaEm { get; set; }
}