using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Data.EFCore
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;

        public UnitOfWorkBase(TContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}