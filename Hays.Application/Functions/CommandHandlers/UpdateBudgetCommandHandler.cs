using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Unit>
    {
        private readonly IBudgetsService _budgetsService;

        public UpdateBudgetCommandHandler(IBudgetsService budgetsService)
        {
            _budgetsService = budgetsService;
        }

        public async Task<Unit> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken)
        {
            await _budgetsService.UpdateBudgetAsync(command);
            return Unit.Value;
        }
    }
}
