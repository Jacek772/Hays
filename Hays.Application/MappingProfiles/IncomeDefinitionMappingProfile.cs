using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Domain.Entities;

namespace Hays.Application.MappingProfiles
{
    public class IncomeDefinitionMappingProfile : Profile
    {
        public IncomeDefinitionMappingProfile()
        {
            CreateMap<IncomeDefinition, IncomeDefinitionDTO>();
            CreateMap<CreateIncomeDefinitionCommand, IncomeDefinition>();
        }
    }
}
