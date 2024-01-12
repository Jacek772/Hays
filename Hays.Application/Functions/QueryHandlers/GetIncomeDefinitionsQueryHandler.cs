using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetIncomeDefinitionsQueryHandler : IRequestHandler<GetIncomeDefinitionsQuery, List<IncomeDefinitionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IIncomeDefinitionsService _incomeDefinitionsService;

        public GetIncomeDefinitionsQueryHandler(IMapper mapper, IIncomeDefinitionsService incomeDefinitionsService)
        {
            _mapper = mapper;
            _incomeDefinitionsService = incomeDefinitionsService;
        }

        public async Task<List<IncomeDefinitionDTO>> Handle(GetIncomeDefinitionsQuery request, CancellationToken cancellationToken)
        {
            List<IncomeDefinition> incomeDefinitions = await _incomeDefinitionsService.GetIncomeDefinitionsAsync();
            return _mapper.Map<List<IncomeDefinition>, List<IncomeDefinitionDTO>>(incomeDefinitions);
        }
    }
}
