using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateSpendingGoalCommandHandler : IRequestHandler<CreateSpendingGoalCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ISpendingGoalsService _spendingGoalsService;

        public CreateSpendingGoalCommandHandler(IMapper mapper, ISpendingGoalsService spendingGoalsService)
        {
            _mapper = mapper;
            _spendingGoalsService = spendingGoalsService;
        }

        public async Task<Unit> Handle(CreateSpendingGoalCommand request, CancellationToken cancellationToken)
        {
            SpendingGoal spendingGoal = _mapper.Map<CreateSpendingGoalCommand, SpendingGoal>(request);
            await _spendingGoalsService.CreateSpendingGoalAsync(spendingGoal);
            return Unit.Value;
        }
    }
}


