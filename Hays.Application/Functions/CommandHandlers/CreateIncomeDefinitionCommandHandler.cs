using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateIncomeDefinitionCommandHandler : IRequestHandler<CreateIncomeDefinitionCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IIncomeDefinitionsService _incomeDefinitionsService;

        public CreateIncomeDefinitionCommandHandler(IMapper mapper, IIncomeDefinitionsService incomeDefinitionsService)
        {
            _mapper = mapper;
            _incomeDefinitionsService = incomeDefinitionsService;
        }

        public async Task<Unit> Handle(CreateIncomeDefinitionCommand request, CancellationToken cancellationToken)
        {
            IncomeDefinition incomeDefinition = _mapper.Map<CreateIncomeDefinitionCommand, IncomeDefinition>(request);
            await _incomeDefinitionsService.CreateIncomeDefinitionAsync(incomeDefinition);
            return Unit.Value;
        }
    }
}
