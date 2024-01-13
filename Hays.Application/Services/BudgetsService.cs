using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static Hays.Domain.Entities.Budget;

namespace Hays.Application.Services
{
    public class BudgetsService : IBudgetsService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BudgetsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Budget>> GetBudgetsAsync(GetUserBudgetsQuery query)
        {
            IQueryable<Budget> budgets = _applicationDbContext.Budgets
                .Include(x => x.Incomes)
                .ThenInclude(x => x.Definition)
                .Include(x => x.Expenses)
                .ThenInclude(x => x.Definition)
                .Include(x => x.Children);

            if(query.UserId is int userId)
            {
                budgets = budgets.Where(x => x.UserId == userId);
            }

            if (query.BudgetType is BudgetType budgetType)
            {
                budgets = budgets.Where(x => x.Type == budgetType);
            }

            if (query.DateFrom is DateTime dateFrom)
            {
                budgets = budgets.Where(x => x.DateTo >= dateFrom);
            }

            if (query.DateFrom is DateTime dateTo)
            {
                budgets = budgets.Where(x => x.DateFrom <= dateTo);
            }

            List<Budget> budgetsA = await budgets.ToListAsync();

            return await budgets.ToListAsync();
        }

        public async Task CreateBudgetAsync(Budget budget)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Budgets.Add(budget);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Budget create error");
                }
            }
        }

        public async Task UpdateBudgetAsync(UpdateBudgetCommand updateBudgetCommand)
        {
            Budget budget = await _applicationDbContext.Budgets.FirstOrDefaultAsync(x => x.Id == updateBudgetCommand.Id);
            if (budget is null)
            {
                throw new BadRequestException("Expense does not exists");
            }

            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    if(updateBudgetCommand.BudgetValue is decimal budgetValue)
                    {
                        budget.BudgetValue = budgetValue;
                    }

                    if(updateBudgetCommand.State is BudgetState state)
                    {
                        budget.State = state;
                    }

                    _applicationDbContext.Budgets.Update(budget);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Budget update error");
                }
            }
        }

        public async Task DeleteBudgetAsync(int budgetId)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Budgets.Remove(new Budget { Id = budgetId });
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Budget delete error");
                }
            }
        }

        public async Task<Budget> GetMonthlyBudgetAsync(int userId, DateTime date)
        {
            return await _applicationDbContext.Budgets
                .FirstOrDefaultAsync(x => x.Type == BudgetType.Monthly
                    && x.DateFrom <= date
                    && x.DateTo >= date
                    && x.UserId == userId);
        }

        public async Task<Budget> GetYearlyBudgetAsync(int userId, int year)
        {
            return await _applicationDbContext.Budgets
                .FirstOrDefaultAsync(x => x.UserId == userId && x.DateFrom.Year == year && x.Type == BudgetType.Yearly);
        }

        public async Task InitBudgetsForUser(User user)
        {
            List<Budget> monthlyBudgets = new List<Budget>();
            for (int i = 1; i <= 12; i++)
            {
                Budget newMonthlyBudget = new()
                {
                    DateFrom = new DateTime(DateTime.Now.Year, i, 01),
                    DateTo = new DateTime(DateTime.Now.Year, i, DateTime.DaysInMonth(DateTime.Now.Year, i)),
                    State = BudgetState.Open,
                    Type = BudgetType.Monthly,
                    UserId = user.Id,
                    User = user,
                };
                monthlyBudgets.Add(newMonthlyBudget);
            }

            Budget newYearlyBudget = new()
            {
                DateFrom = new DateTime(DateTime.Now.Year, 01, 01),
                DateTo = new DateTime(DateTime.Now.Year, 12, 31),
                State = BudgetState.Open,
                Type = BudgetType.Yearly,
                UserId = user.Id,
                Children = monthlyBudgets,
                User = user,
            };
            await CreateBudgetAsync(newYearlyBudget);
        }

        public async Task<int> GetBudgetIdForExpenseAsync(User user, Expense expense)
        {
            Budget monthlyBudget = await GetMonthlyBudgetAsync(user.Id, expense.Date);

            if (monthlyBudget is not null)
            {
                return monthlyBudget.Id;
            }
            else
            {
                List<Budget> monthlyBudgets = new List<Budget>();
                for (int i = 1; i <= 12; i++)
                {
                    Budget newMonthlyBudget = new()
                    {
                        DateFrom = new DateTime(expense.Date.Year, i, 1),
                        DateTo = new DateTime(expense.Date.Year, i, DateTime.DaysInMonth(expense.Date.Year, i)),
                        State = BudgetState.Open,
                        Type = BudgetType.Monthly,
                        UserId = user.Id,
                        User = user,
                    };
                    monthlyBudgets.Add(newMonthlyBudget);
                }

                Budget newYearlyBudget = new()
                {
                    DateFrom = new DateTime(expense.Date.Year, 01, 01),
                    DateTo = new DateTime(expense.Date.Year, 12, 31),
                    State = BudgetState.Open,
                    Type = BudgetType.Yearly,
                    UserId = user.Id,
                    Children = monthlyBudgets,
                    User = user,
                };
                await CreateBudgetAsync(newYearlyBudget);

                monthlyBudget = await GetMonthlyBudgetAsync(user.Id, expense.Date);
                return monthlyBudget.Id;
            }
        }

        public async Task<int> GetBudgetIdForIncomeAsync(User user, Income income)
        {
            Budget monthlyBudget = await GetMonthlyBudgetAsync(user.Id, income.Date);

            if (monthlyBudget is not null)
            {
                return monthlyBudget.Id;
            }
            else
            {
                List<Budget> monthlyBudgets = new List<Budget>();
                for (int i = 1; i <= 12; i++)
                {
                    Budget newMonthlyBudget = new()
                    {
                        DateFrom = new DateTime(income.Date.Year, i, 1),
                        DateTo = new DateTime(income.Date.Year, i, DateTime.DaysInMonth(income.Date.Year, i)),
                        State = BudgetState.Open,
                        Type = BudgetType.Monthly,
                        UserId = user.Id,
                        User = user,
                    };
                    monthlyBudgets.Add(newMonthlyBudget);
                }

                Budget newYearlyBudget = new()
                {
                    DateFrom = new DateTime(income.Date.Year, 01, 01),
                    DateTo = new DateTime(income.Date.Year, 12, 31),
                    State = BudgetState.Open,
                    Type = BudgetType.Yearly,
                    UserId = user.Id,
                    Children = monthlyBudgets,
                    User = user,
                };
                await CreateBudgetAsync(newYearlyBudget);

                monthlyBudget = await GetMonthlyBudgetAsync(user.Id, income.Date);
                return monthlyBudget.Id;
            }
        }

        public async Task<bool> ExistsAsync()
        {
            return await _applicationDbContext.Budgets.AnyAsync();
        }
    }
}
