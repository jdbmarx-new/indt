using ContratacaoApi.Domain.Entities;

namespace ContratacaoApi.Application.Interfaces
{
    public interface IContratacaoService
    {
        Task<Contratacao> ContratarPropostaAsync(int propostaId);

        Task<Contratacao?> ObterPorIdAsync(int id);

        Task<IEnumerable<Contratacao>> ListarAsync();
    }
}