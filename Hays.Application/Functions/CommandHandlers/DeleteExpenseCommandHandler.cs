using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Unit>
    {
        private readonly IExpensesService _expensesService;

        public DeleteExpenseCommandHandler(IExpensesService expensesService, IMapper mapper)
        {
            _expensesService = expensesService;
        }

        public async Task<Unit> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            await _expensesService.DeleteExpenseAsync(request.ExpenseId);
            return Unit.Value;
        }
    }
}
