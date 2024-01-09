using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Domain.Entities;
using System.Threading.Tasks;
using static Hays.Domain.Entities.Budget;

namespace Hays.Application.Services.Abstracts
{
    public interface IBudgetsService
    {
        Task<List<Budget>> GetBudgetsAsync(GetUserBudgetsQuery query);
        Task<List<Budget>> GetUserBudgetsAsync(int userId);
        Task<Budget> GetYearlyBudgetAsync(int userId, int year);
        Task<List<Budget>> GetYearlyBudgetsAsync(int userId);
        Task<List<Budget>> GetYearlyBudgetsByDateAsync(int userId, DateTime? dateFrom, DateTime? dateTo);
        Task<Budget> GetMonthlyBudgetAsync(int userId, DateTime date);
        Task<List<Budget>> GetMonthlyBudgetsAsync(int userId);
        Task<List<Budget>> GetMonthlyBudgetsByDateAsync(int userId, DateTime? dateFrom, DateTime? dateTo);
        Task<List<Budget>> GetBudgetsByDateAsync(int userId, DateTime? dateFrom, DateTime? dateTo);
        Task CreateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int budgetId);
        Task UpdateBudgetAsync(UpdateBudgetCommand command);
        Task<bool> ExistsBudgetAsync(DateTime dateFrom, DateTime dateTo, BudgetType budgetType, int userId);
        Task<int> GetBudgetIdForExpenseAsync(User user, Expense expense);
        Task<int> GetBudgetIdForIncomeAsync(User user, Income expense);
        Task<bool> ExistsAsync();
    }
}
