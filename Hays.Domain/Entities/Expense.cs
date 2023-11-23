namespace Hays.Domain.Entities
{
    public class Expense : BaseEntitiy
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public int DefinitionId { get; set; }
        public ExpenseDefinition Definition { get; set; } = default!;

        public int BudgetId { get; set; }
        public Budget Budget { get; set; } = default!;
    }
}
