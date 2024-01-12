using Hays.Domain.Entities.Abstracts;

namespace Hays.Domain.Entities
{
    public class Budget : BaseEntitiy
    {
        public enum BudgetState
        {
            Open,
            Closed,
            Cancelled
        }

        public enum BudgetType
        {
            Yearly,
            Monthly
        }

        public DateTime DateFrom { get; set; } = DateTime.Now;
        public DateTime DateTo { get; set; } = DateTime.Now;
        public decimal PlannedExpenses { get; set; }
        public decimal PlannedIncome { get; set; }
        public BudgetState State { get; set; } = BudgetState.Open;
        public BudgetType Type { get; set; } = BudgetType.Yearly;
        public decimal BudgetValue { get; set; }

        public int? ParentId { get; set; }
        public Budget? Parent { get; set; }

        public ICollection<Budget> Children { get; set; } = default!;

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public ICollection<Income> Incomes { get; set; } = default!;
        public ICollection<Expense> Expenses { get; set; } = default!;
    }
}
