using PropostaApi.Domain.Entities;
using PropostaApi.Dto;

namespace PropostaApi.Application.Interfaces;

public interface IPropostaService
{
    Task<Proposta> CriarAsync(CadastroPropostaDto proposta);

    Task<IEnumerable<Proposta>> ListarAsync();

    Task<Proposta?> AlterarStatusAsync(int id, PropostaStatus novoStatus);

    Task<Proposta?> ObterPorIdAsync(int id);
}