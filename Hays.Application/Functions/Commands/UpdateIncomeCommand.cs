using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class UpdateIncomeCommand : IRequest<Unit>
    {
        public int IncomeId { get; set; }
        public DateTime? Date { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
    }
}
