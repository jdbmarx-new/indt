using Common.Infrastructure.Data.EFCore;
using ContratacaoApi.Application.Interfaces;
using ContratacaoApi.Domain.Entities;
using ContratacaoApi.Infrastructure.Persistence;

namespace ContratacaoApi.Infrastructure
{
    public class ContratacaoRepository(ContratacaoDbContext context)
        : RepositoryBase<Contratacao, ContratacaoDbContext>(context), IContratacaoRepository
    {
    }
}