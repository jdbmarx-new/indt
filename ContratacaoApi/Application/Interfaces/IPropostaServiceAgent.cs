using ContratacaoApi.Application.Interfaces.Dto;

namespace ContratacaoApi.Application.Interfaces
{
    public interface IPropostaServiceAgent
    {
        Task<PropostaDto?> GetPropostaAsync(int propostaId);
    }
}