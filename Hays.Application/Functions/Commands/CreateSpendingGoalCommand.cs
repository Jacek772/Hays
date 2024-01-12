using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class CreateSpendingGoalCommand : IRequest<Unit>
    {
        public DateTime? Date { get; set; }
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
        public int? UserId { get; set; }
    }
}
