using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteSpendingGoalCommandHandler : IRequestHandler<DeleteSpendingGoalCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ISpendingGoalsService _spendingGoalsService;

        public DeleteSpendingGoalCommandHandler(IMapper mapper, ISpendingGoalsService spendingGoalsService)
        {
            _mapper = mapper;
            _spendingGoalsService = spendingGoalsService;
        }

        public async Task<Unit> Handle(DeleteSpendingGoalCommand request, CancellationToken cancellationToken)
        {
            await _spendingGoalsService.DeleteSpendingGoalAsync(request.Id);
            return Unit.Value;
        }
    }
}
