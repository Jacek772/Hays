using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class UpdateBudgetCommand : IRequest<Unit>
    {
        public int BudgetId { get; set; }
        public decimal? PlannedExpenses { get; set; }
        public decimal? PlannedIncome { get; set; }
        public int? State { get; set; }
    }
}
