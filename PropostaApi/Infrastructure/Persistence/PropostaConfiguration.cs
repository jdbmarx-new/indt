using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropostaApi.Domain.Entities;

namespace PropostaApi.Infrastructure.Persistence
{
    public class PropostaConfiguration : IEntityTypeConfiguration<Proposta>
    {
        public void Configure(EntityTypeBuilder<Proposta> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Cliente)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Produto)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(p => p.CriadaEm)
                   .IsRequired();
        }
    }
}
