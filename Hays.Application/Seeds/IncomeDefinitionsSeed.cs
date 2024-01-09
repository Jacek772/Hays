using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;

namespace Hays.Application.Seeds
{
    public class IncomeDefinitionsSeed : ISeed
    {
        private static readonly IncomeDefinition[] _incomeDefinitionsData = {
            new IncomeDefinition
            {
                Name = "Salary",
                Description = "Monthly salary"
            },
            new IncomeDefinition
            {
                Name = "Benefit",
                Description = "Monthly denefit"
            },
            new IncomeDefinition
            {
                Name = "Casual job",
                Description = "Monthly casual job"
            }
        };

        private readonly IIncomeDefinitionsService _incomeDefinitionsService;

        public IncomeDefinitionsSeed(IIncomeDefinitionsService incomeDefinitionsService)
        {
            _incomeDefinitionsService = incomeDefinitionsService;
        }

        public async Task Seed()
        {
            if(await _incomeDefinitionsService.ExistsAsync())
            {
                return;
            }

            await _incomeDefinitionsService.CreateIncomeDefinitionsAsync(_incomeDefinitionsData);
        }
    }
}
