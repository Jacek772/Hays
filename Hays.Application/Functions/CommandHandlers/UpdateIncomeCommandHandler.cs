using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand, Unit>
    {
        private readonly IIncomesService _incomesService;

        public UpdateIncomeCommandHandler(IIncomesService incomesService)
        {
            _incomesService = incomesService;
        }

        public async Task<Unit> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
        {
            await _incomesService.UpdateIncomeAsync(request);
            return Unit.Value;
        }
    }
}
