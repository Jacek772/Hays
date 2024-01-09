using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hays.Infrastructure.Configuration
{
    internal class ExpenseDefinitionConfiguration : IEntityTypeConfiguration<ExpenseDefinition>
    {
        public void Configure(EntityTypeBuilder<ExpenseDefinition> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder
                .Property(x => x.Description)
                .HasMaxLength(255);

            // Relationships
            builder
                .HasMany(x => x.Expenses)
                .WithOne(y => y.Definition)
                .HasForeignKey(y => y.DefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
