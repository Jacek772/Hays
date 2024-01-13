using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hays.Infrastructure.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder
               .Property(x => x.Salt)
               .IsRequired()
               .HasMaxLength(255);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder
                .Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(25);

            // Indexes
            builder
                .HasIndex(x => x.Email)
                .IsUnique();

            // Relationships
            builder
                .HasMany(x => x.Budgets)
                .WithOne(y => y.User)
                .HasForeignKey(y => y.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
