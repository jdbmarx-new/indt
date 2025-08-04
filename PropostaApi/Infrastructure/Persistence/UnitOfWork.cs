using Common.Infrastructure.Data.EFCore;

namespace PropostaApi.Infrastructure.Persistence;

public class UnitOfWork : UnitOfWorkBase<PropostaDbContext>
{
    public UnitOfWork(PropostaDbContext context) : base(context)
    {
    }
}