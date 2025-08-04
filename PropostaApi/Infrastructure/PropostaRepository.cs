using Common.Infrastructure.Data.EFCore;
using PropostaApi.Application.Interfaces;
using PropostaApi.Domain.Entities;
using PropostaApi.Infrastructure.Persistence;

namespace PropostaApi.Infrastructure
{
    public class PropostaRepository(PropostaDbContext context)
        : RepositoryBase<Proposta, PropostaDbContext>(context), IPropostaRepository
    {
    }
}