using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hays.Infrastructure.Configuration
{
    internal class IncomeConfiguration : IEntityTypeConfiguration<Income>
    {
        public void Configure(EntityTypeBuilder<Income> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Date)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder
                .Property(x => x.Description)
                .IsRequired(false)
                .HasMaxLength(255);

            builder
                .Property(x => x.Amount)
                .HasPrecision(14, 2)
                .IsRequired();

            //Relationships
            builder
                .HasOne(x => x.Definition)
                .WithMany(y => y.Incomes)
                .HasForeignKey(x => x.DefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Budget)
                .WithMany(y => y.Incomes)
                .HasForeignKey(y => y.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
