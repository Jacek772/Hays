using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Domain.Entities;

namespace Hays.Application.MappingProfiles
{
    public class ExpenseDefinitionMappingProfile : Profile
    {
        public ExpenseDefinitionMappingProfile()
        {
            CreateMap<ExpenseDefinition, ExpenseDefinitionDTO>();
            CreateMap<CreateExpenseDefinitionCommand, ExpenseDefinition>();
        }
    }
}
