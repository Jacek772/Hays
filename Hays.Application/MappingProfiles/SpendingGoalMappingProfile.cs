using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Domain.Entities;

namespace Hays.Application.MappingProfiles
{
    public class SpendingGoalMappingProfile : Profile
    {
        public SpendingGoalMappingProfile()
        {
            CreateMap<SpendingGoal, SpendingGoalDTO>();
            CreateMap<CreateSpendingGoalCommand, SpendingGoal>();
        }
    }
}
