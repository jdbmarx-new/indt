using ContratacaoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContratacaoApi.Infrastructure.Persistence
{
    public class ContratacaoDbContext(DbContextOptions<ContratacaoDbContext> options)
        : DbContext(options)
    {
        public DbSet<Contratacao> Contratacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContratacaoConfiguration());
        }
    }
}