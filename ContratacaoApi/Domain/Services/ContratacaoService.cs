using Common.Domain.Interfaces;
using ContratacaoApi.Application.Interfaces;
using ContratacaoApi.Application.Interfaces.Dto;
using ContratacaoApi.Domain.Entities;

namespace ContratacaoApi.Domain.Services
{
    public class ContratacaoService : IContratacaoService
    {
        private readonly IContratacaoRepository _repository;
        private readonly IPropostaServiceAgent _serviceAgent;
        private readonly IUnitOfWork _unitOfWork;

        public ContratacaoService(IContratacaoRepository repository, IPropostaServiceAgent serviceAgent, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _serviceAgent = serviceAgent;
            _unitOfWork = unitOfWork;
        }

        public async Task<Contratacao> ContratarPropostaAsync(int propostaId)
        {
            var proposta = await _serviceAgent.GetPropostaAsync(propostaId).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Proposta com ID {propostaId} não encontrada.");

            var status = proposta.Status;

            if (status != PropostaStatus.Aprovada)
            {
                throw new InvalidOperationException("Proposta não está aprovada.");
            }

            var contratacao = new Contratacao
            {
                PropostaId = propostaId,
                DataContratacao = DateTime.UtcNow
            };

            await _repository.AddAsync(contratacao).ConfigureAwait(false);
            await _unitOfWork.Commit().ConfigureAwait(false);
            return contratacao;
        }

        public Task<IEnumerable<Contratacao>> ListarAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Contratacao?> ObterPorIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}