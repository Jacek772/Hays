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

        public async Task<bool> ExistsIncomeDefinitionAsync(string name)
        {
            IncomeDefinition incomeDefinition = await _applicationDbContext.IncomeDefinitions
                .FirstOrDefaultAsync(x => x.Name == name);
            return incomeDefinition is not null;
        }

        public async Task<IncomeDefinition> GetIncomeDefinitionAsync(int incomeDefinitionId)
        {
            return await _applicationDbContext.IncomeDefinitions
                .FirstOrDefaultAsync(x => x.Id == incomeDefinitionId);
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
