using Common.Infrastructure.Data.EFCore;

namespace ContratacaoApi.Infrastructure.Persistence;

public class UnitOfWork(ContratacaoDbContext context)
    : UnitOfWorkBase<ContratacaoDbContext>(context)
{
}