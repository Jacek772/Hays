using Hays.Application.Configuration;
using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;

namespace Hays.Application.Seeds
{
    public class IncomesSeed : ISeed
    {
        private static readonly Dictionary<string, Income[]> _incomesPerDefinitionNamesData = new Dictionary<string, Income[]>
        {
            {
                "Salary",
                new Income[]
                {
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 1, 10),
                        Name = "Salary 01.2023",
                        Description = "Monthly main work salary",
                        Amount = 2000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 2, 9),
                        Name = "Salary 02.2023",
                        Description = "Monthly Salary",
                        Amount = 2200m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 2, 9),
                        Name = "Salary bonus",
                        Amount = 500m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 3, 10),
                        Name = "Salary 03.2023",
                        Description = "Monthly Salary",
                        Amount = 2100m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 4, 8),
                        Name = "Salary 04.2023",
                        Description = "Monthly Salary",
                        Amount = 2000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 5, 10),
                        Name = "Salary 05.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 6, 10),
                        Name = "Salary 06.2023",
                        Description = "Monthly Salary",
                        Amount = 2000m
                    }
                }
            },
            {
                "Benefit",
                new Income[]
                {
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 4, 15),
                        Name = "Social benefit",
                        Description = "Social denefit from city",
                        Amount = 1000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 4, 21),
                        Name = "Special benefit",
                        Description = "Special benefit",
                        Amount = 300m
                    },
                }
            },
            {
                "Casual job",
                new Income[]
                {
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 5, 5),
                        Name = "Casual job 05.2023",
                        Description = "Casual job as a cleaner",
                        Amount = 700m
                    }
                }
            }
        };

        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IIncomesService _incomesService;
        private readonly IIncomeDefinitionsService _incomeDefinitionsService;
        private readonly IBudgetsService _budgetsService;
        private readonly IUsersService _usersService;

        public IncomesSeed(AuthenticationConfiguration authenticationConfiguration,
            IIncomesService incomesService,
            IIncomeDefinitionsService incomeDefinitionsService,
            IBudgetsService budgetsService,
            IUsersService usersService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _incomesService = incomesService;
            _incomeDefinitionsService = incomeDefinitionsService;
            _budgetsService = budgetsService;
            _usersService = usersService;
        }

        public async Task Seed()
        {
            if(await _incomesService.ExistsAsync())
            {
                return;
            }

            foreach (var incomesPerDefinitionNameData in _incomesPerDefinitionNamesData)
            {
                string definitonName = incomesPerDefinitionNameData.Key;
                IncomeDefinition incomeDefinition = await _incomeDefinitionsService.GetIncomeDefinitionAsync(definitonName);
                User user = await _usersService.GetUserAsync(_authenticationConfiguration.AdminEmail);

                foreach (Income income in incomesPerDefinitionNameData.Value)
                {
                    income.Definition = incomeDefinition;
                    income.Budget = await _budgetsService.GetMonthlyBudgetAsync(user.Id, income.Date);
                    await _incomesService.CreateIncomeAsync(income);
                }
            }
        }
    }
}
