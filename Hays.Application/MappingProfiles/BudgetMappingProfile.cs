﻿using AutoMapper;
using Hays.Application.DTO;
using Hays.Domain.Entities;

namespace Hays.Application.MappingProfiles
{
    public class BudgetMappingProfile : Profile
    {
        public BudgetMappingProfile()
        {
            CreateMap<Budget, BudgetDTO>()
                .ForMember(m => m.Expenses, x => x.MapFrom(y => y.Expenses))
                .ForMember(m => m.Incomes, x => x.MapFrom(y => y.Incomes));
        }
    }
}
