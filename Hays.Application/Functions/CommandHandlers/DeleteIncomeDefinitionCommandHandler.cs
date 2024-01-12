using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteIncomeDefinitionCommandHandler : IRequestHandler<DeleteIncomeDefinitionCommand, Unit>
    {
        private readonly IIncomeDefinitionsService _incomeDefinitionsService;

        public DeleteIncomeDefinitionCommandHandler(IIncomeDefinitionsService incomeDefinitionsService)
        {
            _incomeDefinitionsService = incomeDefinitionsService;
        }

        public async Task<Unit> Handle(DeleteIncomeDefinitionCommand request, CancellationToken cancellationToken)
        {
            await _incomeDefinitionsService.DeleteIncomeDefinitionAsync(request.Id);
            return Unit.Value;
        }
    }
}
