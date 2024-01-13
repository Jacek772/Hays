using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Hays.Application.Services
{
    public class IncomeDefinitionsService : IIncomeDefinitionsService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IncomeDefinitionsService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<IncomeDefinition>> GetIncomeDefinitionsAsync()
        {
            return await _applicationDbContext.IncomeDefinitions.ToListAsync();
        }

        public async Task CreateIncomeDefinitionAsync(IncomeDefinition incomeDefinition)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    _applicationDbContext.IncomeDefinitions.Add(incomeDefinition);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("IncomeDefinition create error");
                }
            }
        }

        public async Task CreateIncomeDefinitionsAsync(IEnumerable<IncomeDefinition> incomeDefinitions)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    await _applicationDbContext.IncomeDefinitions.AddRangeAsync(incomeDefinitions);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("IncomeDefinitions create error");
                }
            }
        }

        public async Task DeleteIncomeDefinitionAsync(int id)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    IncomeDefinition incomeDefinition = await _applicationDbContext.IncomeDefinitions
                        .FirstOrDefaultAsync(x => x.Id == id);

                    _applicationDbContext.IncomeDefinitions.Remove(incomeDefinition);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("IncomeDefinition delete error");
                }
            }
        }

        public async Task<IncomeDefinition> GetIncomeDefinitionAsync(string name)
        {
            return await _applicationDbContext.IncomeDefinitions
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<bool> ExistsAsync()
        {
            return await _applicationDbContext.IncomeDefinitions.AnyAsync();
        }
    }
}
