using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IExpenseDefinitionsService
    {
        Task<List<ExpenseDefinition>> GetExpenseDefinitionsAsync();
        Task CreateExpenseDefinitionAsync(ExpenseDefinition expenseDefinition);
        Task CreateExpenseDefinitionsAsync(IEnumerable<ExpenseDefinition> expenseDefinitions);
        Task DeleteExpenseDefinitionAsync(int id);
        Task<ExpenseDefinition> GetExpenseDefinitionAsync(string name);
        Task<bool> ExistsAsync();
    }
}
