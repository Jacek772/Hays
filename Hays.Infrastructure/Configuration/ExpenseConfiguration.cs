using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hays.Infrastructure.Configuration
{
    internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
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

            // Relationships
            builder
                .HasOne(x => x.Definition)
                .WithMany(y => y.Expenses)
                .HasForeignKey(y => y.DefinitionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Budget)
                .WithMany(y => y.Expenses)
                .HasForeignKey(y => y.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
