using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteExpenseCommand : IRequest<Unit>
    {
        public int ExpenseId { get; set; }
    }
}
