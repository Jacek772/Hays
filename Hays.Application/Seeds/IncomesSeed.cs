﻿using Hays.Application.Configuration;
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
                        Date = new DateTime(DateTime.Now.Year - 1, 1, 10),
                        Name = "Salary 12.2022",
                        Description = "Monthly main work salary",
                        Amount = 4000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 2, 9),
                        Name = "Salary 01.2023",
                        Description = "Monthly Salary",
                        Amount = 3200m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 2, 9),
                        Name = "Salary bonus",
                        Amount = 500m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 3, 10),
                        Name = "Salary 02.2023",
                        Description = "Monthly Salary",
                        Amount = 2700m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 4, 8),
                        Name = "Salary 03.2023",
                        Description = "Monthly Salary",
                        Amount = 2000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 5, 10),
                        Name = "Salary 04.2023",
                        Description = "Monthly Salary",
                        Amount = 2900m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 6, 10),
                        Name = "Salary 05.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 7, 10),
                        Name = "Salary 06.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 8, 10),
                        Name = "Salary 07.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 9, 10),
                        Name = "Salary 08.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 10, 10),
                        Name = "Salary 09.2023",
                        Description = "Monthly Salary",
                        Amount = 2400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 11, 10),
                        Name = "Salary 10.2023",
                        Description = "Monthly Salary",
                        Amount = 3400m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 12, 10),
                        Name = "Salary 11.2023",
                        Description = "Monthly Salary",
                        Amount = 3800m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year, 01, 10),
                        Name = "Salary 12.2023",
                        Description = "Monthly Salary",
                        Amount = 4100m
                    }
                }
            },
            {
                "Benefit",
                new Income[]
                {
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 4, 15),
                        Name = "Social benefit",
                        Description = "Social denefit from city",
                        Amount = 1000m
                    },
                    new Income
                    {
                        Date = new DateTime(DateTime.Now.Year - 1, 4, 21),
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
                        Date = new DateTime(DateTime.Now.Year - 1, 5, 5),
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
