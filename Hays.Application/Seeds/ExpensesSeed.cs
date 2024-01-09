using Hays.Application.Configuration;
using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;

namespace Hays.Application.Seeds
{
    public class ExpensesSeed : ISeed
    {
        private static readonly Dictionary<string, Expense[]> _expensesPerDefinitionNamesData = new Dictionary<string, Expense[]>
        {
            {
                "Grocery shopping",
                new Expense[] {
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 1, 13),
                        Name = "Shopping",
                        Description = "Shopping",
                        Amount = 200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 2, 17),
                        Name = "Shopping",
                        Description = "Shopping",
                        Amount = 700m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 3, 20),
                        Name = "Shopping",
                        Description = "Shopping",
                        Amount = 50m
                    },
                }
            },
            {
                "Internet",
                new Expense[]
                {
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 1, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 2, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 3, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 4, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 5, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 6, 7),
                        Name = "Internet",
                        Description = "Monthly internet",
                        Amount = 80m
                    },
                }
            },
            {
                "Rent",
                new Expense[] {
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 1, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 2, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 3, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 4, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 5, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                    new Expense
                    {
                        Date = new DateTime(DateTime.Now.Year, 6, 15),
                        Name = "Rent",
                        Description = "Monthly flat rent",
                        Amount = 2200m
                    },
                }
            }
        };

        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IExpensesService _expensesService;
        private readonly IExpenseDefinitionsService _expenseDefinitionsService;
        private readonly IBudgetsService _budgetsService;
        private readonly IUsersService _usersService;

        public ExpensesSeed(AuthenticationConfiguration authenticationConfiguration, 
            IExpensesService expensesService,
            IExpenseDefinitionsService expenseDefinitionsService,
            IBudgetsService budgetsService,
            IUsersService usersService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _expensesService = expensesService;
            _expenseDefinitionsService = expenseDefinitionsService;
            _budgetsService = budgetsService;
            _usersService = usersService;
        }

        public async Task Seed()
        {
            if(await _expensesService.ExistsAsync())
            {
                return;
            }

            foreach (var expensesPerDefinitionNameData in _expensesPerDefinitionNamesData)
            {
                string definitonName = expensesPerDefinitionNameData.Key;
                ExpenseDefinition expenseDefinition = await _expenseDefinitionsService.GetExpenseDefinitionAsync(definitonName);
                User user = await _usersService.GetUserAsync(_authenticationConfiguration.AdminEmail);

                foreach (Expense expense in expensesPerDefinitionNameData.Value)
                {
                    expense.Definition = expenseDefinition;
                    expense.Budget = await _budgetsService.GetMonthlyBudgetAsync(user.Id, expense.Date);
                    await _expensesService.CreateExpenseAsync(expense);
                }
            }
        }
    }
}
