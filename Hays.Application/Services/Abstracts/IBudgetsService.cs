using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Domain.Entities;
using System.Threading.Tasks;

namespace Hays.Application.Services.Abstracts
{
    public interface IBudgetsService
    {
        Task<List<Budget>> GetBudgetsAsync(GetUserBudgetsQuery query);
        Task<Budget> GetYearlyBudgetAsync(int userId, int year);
        Task<Budget> GetMonthlyBudgetAsync(int userId, DateTime date);
        Task CreateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int budgetId);
        Task UpdateBudgetAsync(UpdateBudgetCommand command);
        Task InitBudgetsForUser(User user);
        Task<int> GetBudgetIdForExpenseAsync(User user, Expense expense);
        Task<int> GetBudgetIdForIncomeAsync(User user, Income expense);
        Task<bool> ExistsAsync();
    }
}
