using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class CreateExpenseCommand : IRequest<Unit>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public decimal Amount { get; set; }
        public int DefinitionId { get; set; }

        public int BudgetId { get; set; }
        public int UserId { get; set; }
    }
}
