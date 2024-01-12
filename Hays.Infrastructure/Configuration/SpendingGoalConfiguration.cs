using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hays.Infrastructure.Configuration
{
    internal class SpendingGoalConfiguration : IEntityTypeConfiguration<SpendingGoal>
    {
        public void Configure(EntityTypeBuilder<SpendingGoal> builder)
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
                .HasMaxLength(100);

            builder
                .Property(x => x.Amount)
                .HasPrecision(14, 2)
                .IsRequired();

            // Relationships
            builder
                .HasOne(x => x.User)
                .WithMany(y => y.SpendingGoals)
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
