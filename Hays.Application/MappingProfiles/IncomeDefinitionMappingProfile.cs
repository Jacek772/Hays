using AutoMapper;
using Hays.Application.DTO;
using Hays.Domain.Entities;

namespace Hays.Application.MappingProfiles
{
    public class IncomeDefinitionMappingProfile : Profile
    {
        public IncomeDefinitionMappingProfile()
        {
            CreateMap<IncomeDefinition, IncomeDefinitionDTO>();
        }
    }
}
