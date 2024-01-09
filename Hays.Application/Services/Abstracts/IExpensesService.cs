using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IExpensesService
    {
        Task<List<Expense>> GetExpensesByMonthlyBudgetIdAsync(int monthlyBudgetId);
        Task<List<Expense>> GetExpensesByYearlyBudgetIdAsync(int yearlyBudgetId);
        Task<List<Expense>> GetExpensesAsync(int userId, int page = 0, int pageSize = 0);
        Task<List<Expense>> GetExpensesAsync(int userId, string searchPhrase, int page = 0, int pageSize = 0);
        Task<List<Expense>> GetExpensesAsync(GetUserExpensesQuery query);
        Task<Expense> GetExpenseAsync(int userId, int expenseId);
        Task CreateExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(UpdateExpenseCommand updateExpenseCommand);
        Task DeleteExpenseAsync(int expenseId);
        Task<bool> ExistsExpenseAsync(string definitonName, DateTime dateTime, string name);
        Task<bool> ExistsAsync();
    }
}
