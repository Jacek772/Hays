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
    public class IncomesService : IIncomesService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IncomesService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Income>> GetIncomesByMonthlyBudgetIdAsync(int monthlyBudgetId)
        {
            return await _applicationDbContext.Incomes
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .Where(x => x.Budget.Type == BudgetType.Monthly && x.BudgetId == monthlyBudgetId)
                .ToListAsync();
        }

        public async Task<List<Income>> GetIncomesByYearlyBudgetIdAsync(int yearlyBudgetId)
        {
            return await _applicationDbContext.Incomes
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .Where(x => x.Budget.Type == BudgetType.Monthly && x.Budget.ParentId == yearlyBudgetId)
                .ToListAsync();
        }

        public async Task<List<Income>> GetIncomesAsync(GetUserIncomesQuery query)
        {
            IQueryable<Income> incomes = _applicationDbContext.Incomes
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .Where(x => x.Budget.UserId == query.UserId);

            if (!string.IsNullOrEmpty(query.SearchPhrase) && !string.IsNullOrWhiteSpace(query.SearchPhrase))
            {
                incomes = incomes
                 .Where(x => EF.Functions.Like(x.Date.ToString(), $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Name, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Description, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Definition.Name, $"%{query.SearchPhrase}%")
                     || EF.Functions.Like(x.Definition.Description, $"%{query.SearchPhrase}%")
                 );
            }

            List<Income> incomesList = await incomes.ToListAsync();

            int page = 1;
            if (query.Page is int pageInt)
            {
                page = pageInt;
            }

            int pageSize = 30;
            if (query.PageSize is int pageSizeInt && pageSizeInt > 0)
            {
                pageSize = pageSizeInt;
            }

            int pagesCount = (int)Math.Ceiling(incomesList.Count / (double)pageSize);
            int index = (page - 1) * pageSize;
            if (index >= incomesList.Count)
            {
                return new List<Income>();
            }

            if (index + pageSize >= incomesList.Count)
            {
                return incomesList.GetRange(index, incomesList.Count - index);
            }

            return incomesList.GetRange((page - 1) * pageSize, pageSize);
        }

        public async Task<Income> GetIncomeAsync(int userId, int incomeId)
        {
            return await _applicationDbContext.Incomes
                .Include(x => x.Definition)
                .Include(x => x.Budget)
                .FirstOrDefaultAsync(x => x.Id == incomeId && x.Budget.User.Id == userId);
        }

        public async Task CreateIncomeAsync(Income income)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Incomes.Add(income);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Income create error");
                }
            }
        }

        public async Task DeleteIncomeAsync(int incomeId)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.Incomes.Remove(new Income { Id = incomeId });
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

        public async Task UpdateIncomeAsync(UpdateIncomeCommand updateIncomeCommand)
        {
            Income income = await _applicationDbContext.Incomes.FirstOrDefaultAsync(x => x.Id == updateIncomeCommand.IncomeId);
            if (income is null)
            {
                throw new BadRequestException("Income does not exists");
            }

            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    income.Date = updateIncomeCommand.Date is null ? income.Date : (DateTime)updateIncomeCommand.Date;
                    income.Name = string.IsNullOrEmpty(updateIncomeCommand.Name) ? income.Name : updateIncomeCommand.Name;
                    income.Description = string.IsNullOrEmpty(updateIncomeCommand.Description) ? income.Description : updateIncomeCommand.Description;
                    income.Amount = updateIncomeCommand.Amount is null ? income.Amount : (decimal)updateIncomeCommand.Amount;

                    _applicationDbContext.Incomes.Update(income);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Income update error");
                }
            }
        }

        public async Task <bool> ExistsAsync()
        {
            return await _applicationDbContext.Incomes.AnyAsync();
        }
    }
}
