using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IExpenseDefinitionsService
    {
        Task CreateExpenseDefinitionAsync(ExpenseDefinition expenseDefinition);
        Task CreateExpenseDefinitionsAsync(IEnumerable<ExpenseDefinition> expenseDefinitions);
        Task<ExpenseDefinition> GetExpenseDefinitionAsync(int expenseDefinitionId);
        Task<ExpenseDefinition> GetExpenseDefinitionAsync(string name);
        Task<bool> ExistsExpenseDefinitionAsync(string name);
        Task<bool> ExistsAsync();
    }
}
