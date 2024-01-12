using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hays.Application.Services
{
    public class SpendingGoalsService : ISpendingGoalsService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SpendingGoalsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<SpendingGoal>> GetSpendingGoalsAsync(GetSpendingGoalsQuery query)
        {
            return await _applicationDbContext.SpendingGoals
                .Where(x => x.UserId == query.UserId)
                .ToListAsync();
        }

        public async Task CreateSpendingGoalAsync(SpendingGoal spendingGoal)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    await _applicationDbContext.SpendingGoals.AddAsync(spendingGoal);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("spending goal create error");
                }
            }
        }

        public async Task DeleteSpendingGoalAsync(int id)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.SpendingGoals.Remove(new SpendingGoal { Id = id });
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Spending goal delete error");
                }
            }
        }
    }
}
