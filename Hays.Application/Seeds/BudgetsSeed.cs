using Hays.Application.Configuration;
using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using static Hays.Domain.Entities.Budget;

namespace Hays.Application.Seeds
{
    public class BudgetsSeed : ISeed
    {
        private static readonly Budget[] _budgetsData = {
            // Yearly
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 1, 1),
                DateTo = new DateTime(DateTime.Now.Year, 12, 31),
                PlannedExpenses = 24000m,
                PlannedIncome = 36000m,
                State = BudgetState.Open,
                Type = BudgetType.Yearly,
            },

            // Monthly
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 1, 1),
                DateTo = new DateTime(DateTime.Now.Year, 1, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly,
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 2, 1),
                DateTo = new DateTime(DateTime.Now.Year, 2, 28),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 3, 1),
                DateTo = new DateTime(DateTime.Now.Year, 3, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 4, 1),
                DateTo = new DateTime(DateTime.Now.Year, 4, 30),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 5, 1),
                DateTo = new DateTime(DateTime.Now.Year, 5, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 6, 1),
                DateTo = new DateTime(DateTime.Now.Year, 6, 30),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 7, 1),
                DateTo = new DateTime(DateTime.Now.Year, 7, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 8, 1),
                DateTo = new DateTime(DateTime.Now.Year, 8, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 9, 1),
                DateTo = new DateTime(DateTime.Now.Year, 9, 30),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 10, 1),
                DateTo = new DateTime(DateTime.Now.Year, 10, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 11, 1),
                DateTo = new DateTime(DateTime.Now.Year, 11, 30),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            },
            new Budget
            {
                DateFrom = new DateTime(DateTime.Now.Year, 12, 1),
                DateTo = new DateTime(DateTime.Now.Year, 12, 31),
                PlannedExpenses = 2000m,
                PlannedIncome = 3000m,
                State = BudgetState.Open,
                Type = BudgetType.Monthly
            }
        };

        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IBudgetsService _budgetsService;
        private readonly IUsersService _usersService;

        public BudgetsSeed(AuthenticationConfiguration authenticationConfiguration,
            IBudgetsService budgetsService, IUsersService usersService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _budgetsService = budgetsService;
            _usersService = usersService;
        }

        public async Task Seed()
        {
            if(await _budgetsService.ExistsAsync())
            {
                return;
            }

            User user = await _usersService.GetUserAsync(_authenticationConfiguration.AdminEmail);
            foreach (Budget budget in _budgetsData)
            {
                if (budget.Type == BudgetType.Monthly)
                {
                    Budget budgetYearly = await _budgetsService.GetYearlyBudgetAsync(user.Id, budget.DateFrom.Year);
                    budget.Parent = budgetYearly;
                }

                budget.User = user;
                await _budgetsService.CreateBudgetAsync(budget);
            }
        }
    }
}
