using Hays.Application.Functions.CommandHandlers;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    internal class DeleteExpenseDefinitionCommandHandler : IRequestHandler<DeleteExpenseDefinitionCommand, Unit>
    {
        private readonly IExpenseDefinitionsService _expenseDefinitionsService;

        public DeleteExpenseDefinitionCommandHandler(IExpenseDefinitionsService expenseDefinitionsService)
        {
            _expenseDefinitionsService = expenseDefinitionsService;
        }

        public async Task<Unit> Handle(DeleteExpenseDefinitionCommand request, CancellationToken cancellationToken)
        {
            await _expenseDefinitionsService.DeleteExpenseDefinitionAsync(request.Id);
            return Unit.Value;
        }
    }
}