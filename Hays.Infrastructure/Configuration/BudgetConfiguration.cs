using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hays.Infrastructure.Configuration
{
    internal class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.DateFrom)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder
                .Property(x => x.DateTo)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            builder
               .Property(x => x.BudgetValue)
               .HasPrecision(14, 2)
               .IsRequired();

            builder
                .Property(x => x.State)
                .IsRequired()
                .HasDefaultValue(Budget.BudgetState.Open);

            builder
                 .Property(x => x.Type)
                 .IsRequired()
                 .HasDefaultValue(Budget.BudgetType.Yearly);

            // Relationships
            builder
                .HasOne(x => x.Parent)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(x => x.Children)
                .WithOne(x => x.Parent)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.User)
                .WithMany(y => y.Budgets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(x => x.Incomes)
                .WithOne(y => y.Budget)
                .HasForeignKey(y => y.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Expenses)
                .WithOne(y => y.Budget)
                .HasForeignKey(y => y.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
