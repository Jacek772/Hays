using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetExpenseDefinitionsQueryHandler : IRequestHandler<GetExpenseDefinitionsQuery, List<ExpenseDefinitionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IExpenseDefinitionsService _expenseDefinitionsService;

        public GetExpenseDefinitionsQueryHandler(IMapper mapper, IExpenseDefinitionsService expenseDefinitionsService)
        {
            _mapper = mapper;
            _expenseDefinitionsService = expenseDefinitionsService;
        }

        public async Task<List<ExpenseDefinitionDTO>> Handle(GetExpenseDefinitionsQuery request, CancellationToken cancellationToken)
        {
            List<ExpenseDefinition> expenseDefinitions = await _expenseDefinitionsService.GetExpenseDefinitionsAsync();
            return _mapper.Map<List<ExpenseDefinition>, List<ExpenseDefinitionDTO>>(expenseDefinitions);
        }
    }
}
