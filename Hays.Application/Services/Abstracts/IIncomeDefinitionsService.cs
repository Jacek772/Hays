using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IIncomeDefinitionsService
    {
        Task CreateIncomeDefinitionAsync(IncomeDefinition incomeDefinition);
        Task CreateIncomeDefinitionsAsync(IEnumerable<IncomeDefinition> incomeDefinitions);
        Task<IncomeDefinition> GetIncomeDefinitionAsync(int incomeDefinitionId);
        Task<IncomeDefinition> GetIncomeDefinitionAsync(string name);
        Task<bool> ExistsIncomeDefinitionAsync(string name);
        Task<bool> ExistsAsync();
    }
}
