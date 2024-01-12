using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateExpenseDefinitionCommandHandler : IRequestHandler<CreateExpenseDefinitionCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IExpenseDefinitionsService _expenseDefinitionsService;

        public CreateExpenseDefinitionCommandHandler(IMapper mapper, IExpenseDefinitionsService expenseDefinitionsService)
        {
            _mapper = mapper;
            _expenseDefinitionsService = expenseDefinitionsService;
        }

        public async Task<Unit> Handle(CreateExpenseDefinitionCommand request, CancellationToken cancellationToken)
        {
            ExpenseDefinition expenseDefinition = _mapper.Map<CreateExpenseDefinitionCommand, ExpenseDefinition>(request);
            await _expenseDefinitionsService.CreateExpenseDefinitionAsync(expenseDefinition);
            return Unit.Value;
        }
    }
}
