using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using static Hays.Domain.Entities.Budget;
using Hays.Application.Functions.Query;

namespace Hays.Application.Services
{
    public class ExpensesService : IExpensesService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ExpensesService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Expense>> GetExpensesByMonthlyBudgetIdAsync(int monthlyBudgetId)
        {
            return await _applicationDbContext.Expenses
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .Where(x => x.Budget.Type == BudgetType.Monthly && x.BudgetId == monthlyBudgetId)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesByYearlyBudgetIdAsync(int yearlyBudgetId)
        {
            return await _applicationDbContext.Expenses
            .Include(x => x.Definition)
            .Include(x => x.Budget)
            .Where(x => x.Budget.Type == BudgetType.Monthly && x.Budget.ParentId == yearlyBudgetId)
            .ToListAsync();
        }

        public async Task<List<Expense>> GetExpensesAsync(GetUserExpensesQuery query)
        {
            IQueryable<Expense> expenses = _applicationDbContext.Expenses
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .Where(x => x.Budget.UserId == query.UserId);

            if(!string.IsNullOrEmpty(query.SearchPhrase) && !string.IsNullOrWhiteSpace(query.SearchPhrase))
            {
                expenses = expenses
                 .Where(x => EF.Functions.Like(x.Date.ToString(), $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Name, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Description, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Definition.Name, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Definition.Description, $"%{query.SearchPhrase}%")
                 );
            }

            List<Expense> expensesList = await expenses.ToListAsync();

            int page = 1;
            if (query.Page is int pageInt)
            {
                page = pageInt;
            }

            int pageSize = 30;
            if(query.PageSize is int pageSizeInt && pageSizeInt > 0)
            {
                pageSize = pageSizeInt;
            }

            int pagesCount = (int)Math.Ceiling(expensesList.Count / (double)pageSize);
            int index = (page - 1) * pageSize;
            if (index >= expensesList.Count)
            {
                return new List<Expense>();
            }

            if (index + pageSize >= expensesList.Count)
            {
                return expensesList.GetRange(index, expensesList.Count - index);
            }

            return expensesList.GetRange((page - 1) * pageSize, pageSize);
        }

        public async Task<Expense> GetExpenseAsync(int userId, int expenseId)
        {
            return await _applicationDbContext.Expenses
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .FirstOrDefaultAsync(x => x.Id == expenseId && x.Budget.UserId == userId);
        }

        public async Task CreateExpenseAsync(Expense expense)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Expenses.Add(expense);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Expense create error");
                }
            }
        }

        public async Task DeleteExpenseAsync(int expenseId)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Expenses.Remove(new Expense { Id = expenseId });
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Income delete error");
                }
            }
        }

        public async Task UpdateExpenseAsync(UpdateExpenseCommand updateExpenseCommand)
        {
            Expense expense = await _applicationDbContext.Expenses.FirstOrDefaultAsync(x => x.Id == updateExpenseCommand.ExpenseId);
            if (expense is null)
            {
                throw new BadRequestException("Expense does not exists");
            }

            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    expense.Date = updateExpenseCommand.Date is null ? expense.Date : (DateTime)updateExpenseCommand.Date;
                    expense.Name = string.IsNullOrEmpty(updateExpenseCommand.Name) ? expense.Name : updateExpenseCommand.Name;
                    expense.Description = string.IsNullOrEmpty(updateExpenseCommand.Description) ? expense.Description : updateExpenseCommand.Description;
                    expense.Amount = updateExpenseCommand.Amount is null ? expense.Amount : (decimal)updateExpenseCommand.Amount;

                    _applicationDbContext.Expenses.Update(expense);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Expense update error");
                }
            }
        }

        public async Task<bool> ExistsAsync()
        {
            return await _applicationDbContext.Expenses.AnyAsync();
        }
    }
}
