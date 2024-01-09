using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand, Unit>
    {
        private readonly IIncomesService _incomesService;

        public DeleteIncomeCommandHandler(IIncomesService incomesService)
        {
            _incomesService = incomesService;
        }

        public async Task<Unit> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            await _incomesService.DeleteIncomeAsync(request.IncomeId);
            return Unit.Value;
        }
    }
}
