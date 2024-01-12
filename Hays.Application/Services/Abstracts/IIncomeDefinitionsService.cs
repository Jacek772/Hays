using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IIncomeDefinitionsService
    {
        Task<List<IncomeDefinition>> GetIncomeDefinitionsAsync();
        Task CreateIncomeDefinitionAsync(IncomeDefinition incomeDefinition);
        Task CreateIncomeDefinitionsAsync(IEnumerable<IncomeDefinition> incomeDefinitions);
        Task DeleteIncomeDefinitionAsync(int id);
        Task<IncomeDefinition> GetIncomeDefinitionAsync(int incomeDefinitionId);
        Task<IncomeDefinition> GetIncomeDefinitionAsync(string name);
        Task<bool> ExistsIncomeDefinitionAsync(string name);
        Task<bool> ExistsAsync();
    }
}
