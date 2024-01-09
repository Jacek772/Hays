using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.MappingProfiles
{
    public class ExpenseMappingProfile : Profile
    {
        public ExpenseMappingProfile()
        {
            CreateMap<Expense, ExpenseDTO>()
                .ForMember(m => m.Definition, x => x.MapFrom(y => y.Definition));

            CreateMap<CreateExpenseCommand, Expense>()
                .ForMember(m => m.Date, c => c.MapFrom(x => x.Date ?? DateTime.Now));
        }
    }
}
