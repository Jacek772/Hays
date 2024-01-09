using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;

namespace Hays.Application.Seeds
{
    public class ExpenseDefinitionsSeed : ISeed
    {
        private static readonly ExpenseDefinition[] _expenseDefinitionsData = {
            new ExpenseDefinition
            {
                Name = "Grocery shopping",
                Description = "Grocery shopping for me"
            },
            new ExpenseDefinition
            {
                Name = "Internet",
                Description = "Monthly internet"
            },
            new ExpenseDefinition
            {
                Name = "Rent",
                Description = "Monthly rent"
            },
            new ExpenseDefinition
            {
                Name = "Insurance",
                Description = "Monthly insurance"
            },
            new ExpenseDefinition
            {
                Name = "Fuel",
                Description = "Fuel for vehicles"
            },
            new ExpenseDefinition
            {
                Name = "Bills",
                Description = "Bills"
            },
        };

        private readonly IExpenseDefinitionsService _expenseDefinitionsService;

        public ExpenseDefinitionsSeed(IExpenseDefinitionsService expenseDefinitionsService)
        {
            _expenseDefinitionsService = expenseDefinitionsService;
        }

        public async Task Seed()
        {
            if(await _expenseDefinitionsService.ExistsAsync())
            {
                return;
            }

            await _expenseDefinitionsService.CreateExpenseDefinitionsAsync(_expenseDefinitionsData);
        }
    }
}
