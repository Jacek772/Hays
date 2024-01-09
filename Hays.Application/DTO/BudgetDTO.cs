namespace Hays.Application.DTO
{
    public class BudgetDTO
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
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public BudgetState State { get; set; }
        public BudgetType Type { get; set; }
        public decimal PlannedExpenses { get; set; }
        public decimal PlannedIncome { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }

        public List<IncomeDTO> Incomes { get; set; } = new List<IncomeDTO>();
        public List<ExpenseDTO> Expenses { get; set; } = new List<ExpenseDTO>();
    }
}
