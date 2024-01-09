using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteIncomeCommand : IRequest<Unit>
    {
        public int IncomeId { get; set; }
    }
}
