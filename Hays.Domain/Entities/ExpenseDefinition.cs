using Hays.Domain.Entities.Abstracts;

namespace Hays.Domain.Entities
{
    public class ExpenseDefinition : BaseEntitiy
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public ICollection<Expense> Expenses { get; set; } = default!;
    }
}
