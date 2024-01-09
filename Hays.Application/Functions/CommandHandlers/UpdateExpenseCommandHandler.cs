using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Unit>
    {
        private readonly IExpensesService _expensesService;

        public UpdateExpenseCommandHandler(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        public async Task<Unit> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            await _expensesService.UpdateExpenseAsync(request);
            return Unit.Value;
        }
    }
}
