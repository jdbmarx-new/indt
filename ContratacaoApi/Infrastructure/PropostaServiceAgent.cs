using Common.Infrastructure.ServiceAgent.Http;
using ContratacaoApi.Application.Interfaces;
using ContratacaoApi.Application.Interfaces.Dto;

namespace ContratacaoApi.Infrastructure
{
    public class PropostaServiceAgent(ILogger<PropostaServiceAgent> logger, HttpClient client)
        : ServiceAgentBase(logger, client, true), IPropostaServiceAgent
    {
        public Task<PropostaDto?> GetPropostaAsync(int propostaId)
        {
            return GetAsync<PropostaDto?>($"api/proposta/{propostaId}");
        }
    }
}