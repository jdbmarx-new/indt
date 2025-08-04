using Microsoft.EntityFrameworkCore;
using PropostaApi.Domain.Entities;

namespace PropostaApi.Infrastructure.Persistence;

public class PropostaDbContext(DbContextOptions<PropostaDbContext> options)
    : DbContext(options)
{
    public DbSet<Proposta> Propostas => Set<Proposta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proposta>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Cliente).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Produto).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Status).HasConversion<string>();
            entity.Property(p => p.CriadaEm).IsRequired();
        });
    }
}