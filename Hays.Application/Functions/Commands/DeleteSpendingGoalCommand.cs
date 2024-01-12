using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteSpendingGoalCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
