using Hays.Application.Functions.Query;
using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface ISpendingGoalsService
    {
        Task CreateSpendingGoalAsync(SpendingGoal spendingGoal);
        Task<List<SpendingGoal>> GetSpendingGoalsAsync(GetSpendingGoalsQuery query);
        Task DeleteSpendingGoalAsync(int id);
    }
}
