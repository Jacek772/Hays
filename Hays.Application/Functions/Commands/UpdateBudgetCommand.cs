using MediatR;
using static Hays.Domain.Entities.Budget;

namespace Hays.Application.Functions.Commands
{
    public class UpdateBudgetCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public decimal? PlannedExpenses { get; set; }
        public decimal? PlannedIncome { get; set; }
        public decimal BudgetValue { get; set; }
        public BudgetState? State { get; set; }
    }
}
