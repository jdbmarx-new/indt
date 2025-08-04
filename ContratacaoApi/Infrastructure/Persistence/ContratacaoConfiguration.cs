using ContratacaoApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContratacaoApi.Infrastructure.Persistence
{
    public class ContratacaoConfiguration : IEntityTypeConfiguration<Contratacao>
    {
        public void Configure(EntityTypeBuilder<Contratacao> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.DataContratacao)
                   .IsRequired();
        }
    }
}