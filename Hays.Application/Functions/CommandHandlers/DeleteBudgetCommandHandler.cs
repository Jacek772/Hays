using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Unit>
    {
        private readonly IBudgetsService _budgetsService;

        public DeleteBudgetCommandHandler(IBudgetsService budgetsService, IMapper mapper)
        {
            _budgetsService = budgetsService;
        }

        public async Task<Unit> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
        {
            await _budgetsService.DeleteBudgetAsync(request.BudgetId);
            return Unit.Value;
        }
    }
}
