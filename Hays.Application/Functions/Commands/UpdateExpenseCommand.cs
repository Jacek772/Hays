using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class UpdateExpenseCommand : IRequest<Unit>
    {
        public int ExpenseId { get; set; }
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
    }
}
