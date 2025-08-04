using AutoMapper;
using Common.Domain.Interfaces;
using PropostaApi.Application.Interfaces;
using PropostaApi.Domain.Entities;
using PropostaApi.Dto;

namespace PropostaApi.Domain.Services;

public class PropostaService : IPropostaService
{
    private readonly IPropostaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropostaService(IPropostaRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Proposta?> ObterPorIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<Proposta> CriarAsync(CadastroPropostaDto proposta)
    {
        var propostaEntity = _mapper.Map<Proposta>(proposta);

        await _repository.AddAsync(propostaEntity).ConfigureAwait(false);
        await _unitOfWork.Commit().ConfigureAwait(false);
        return propostaEntity;
    }

    public Task<IEnumerable<Proposta>> ListarAsync() => _repository.GetAllAsync();

    public async Task<Proposta?> AlterarStatusAsync(int id, PropostaStatus novoStatus)
    {
        var proposta = await _repository.GetByIdAsync(id).ConfigureAwait(false);
        if (proposta is null)
        {
            return null;
        }

        proposta.Status = novoStatus;
        await _unitOfWork.Commit().ConfigureAwait(false);
        return proposta;
    }
}