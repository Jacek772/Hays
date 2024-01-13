using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IExpensesService
    {
        Task<List<Expense>> GetExpensesByMonthlyBudgetIdAsync(int monthlyBudgetId);
        Task<List<Expense>> GetExpensesByYearlyBudgetIdAsync(int yearlyBudgetId);
        Task<List<Expense>> GetExpensesAsync(GetUserExpensesQuery query);
        Task<Expense> GetExpenseAsync(int userId, int expenseId);
        Task CreateExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(UpdateExpenseCommand updateExpenseCommand);
        Task DeleteExpenseAsync(int expenseId);
        Task<bool> ExistsAsync();
    }
}
