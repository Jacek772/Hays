using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hays.Application.Services
{
    public class ExpenseDefinitionsService : IExpenseDefinitionsService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ExpenseDefinitionsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<ExpenseDefinition>> GetExpenseDefinitionsAsync()
        {
            return await _applicationDbContext.ExpenseDefinitions.ToListAsync();
        }

        public async Task CreateExpenseDefinitionAsync(ExpenseDefinition expenseDefinition)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.ExpenseDefinitions.Add(expenseDefinition);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("ExpenseDefinition create error");
                }
            }
        }

        public async Task CreateExpenseDefinitionsAsync(IEnumerable<ExpenseDefinition> expenseDefinitions)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    await _applicationDbContext.ExpenseDefinitions.AddRangeAsync(expenseDefinitions);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("ExpenseDefinition create error");
                }
            }
        }

        public async Task DeleteExpenseDefinitionAsync(int id)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    ExpenseDefinition expenseDefinition = await _applicationDbContext.ExpenseDefinitions
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _applicationDbContext.ExpenseDefinitions.Remove(expenseDefinition);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("ExpenseDefinition delete error");
                }
            }
        }

        public async Task<bool> ExistsExpenseDefinitionAsync(string name)
        {
            ExpenseDefinition expenseDefinition = await _applicationDbContext.ExpenseDefinitions
                .FirstOrDefaultAsync(x => x.Name == name);
            return expenseDefinition is not null;
        }

        public async Task<ExpenseDefinition> GetExpenseDefinitionAsync(int expenseDefinitionId)
        {
            return await _applicationDbContext.ExpenseDefinitions
                .FirstOrDefaultAsync(x => x.Id == expenseDefinitionId);
        }

        public async Task<ExpenseDefinition> GetExpenseDefinitionAsync(string name)
        {
            return await _applicationDbContext.ExpenseDefinitions
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> ExistsAsync()
        {
            return await _applicationDbContext.ExpenseDefinitions.AnyAsync();
        }
    }
}
