using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hays.Infrastructure.Configuration
{
    internal class IncomeDefinitionConfiguration : IEntityTypeConfiguration<IncomeDefinition>
    {
        public void Configure(EntityTypeBuilder<IncomeDefinition> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder
                .Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(255);

            // Relationships
            builder
                .HasMany(x => x.Incomes)
                .WithOne(y => y.Definition)
                .HasForeignKey(y => y.DefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
