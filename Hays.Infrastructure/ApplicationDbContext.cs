using Hays.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hays.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public ApplicationDbContext(DbContextOptions options, DatabaseConfiguration databaseConfiguration) : base(options)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<ExpenseDefinition> ExpenseDefinitions { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<IncomeDefinition> IncomeDefinitions { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<SpendingGoal> SpendingGoals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_databaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
