using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IIncomesService
    {
        Task<List<Income>> GetIncomesByMonthlyBudgetIdAsync(int monthlyBudgetId);
        Task<List<Income>> GetIncomesByYearlyBudgetIdAsync(int yearlyBudgetId);
        Task<List<Income>> GetIncomesAsync(int userId, int page = 0, int pageSize = 0);
        Task<List<Income>> GetIncomesAsync(int userId, string searchPhrase, int page = 0, int pageSize = 0);
        Task<Income> GetIncomeAsync(int userId, int incomeId);
        Task<List<Income>> GetIncomesAsync(GetUserIncomesQuery query);
        Task CreateIncomeAsync(Income income);
        Task UpdateIncomeAsync(UpdateIncomeCommand updateIncomeCommand);
        Task DeleteIncomeAsync(int incomeId);
        Task<bool> ExistsIncomeAsync(string definitonName, DateTime dateTime, string name);
        Task<bool> ExistsAsync();
    }
}
