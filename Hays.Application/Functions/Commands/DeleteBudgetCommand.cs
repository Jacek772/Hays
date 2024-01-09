using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteBudgetCommand : IRequest<Unit>
    {
        public int BudgetId { get; set; }
    }
}
