using Hays.Domain.Entities.Abstracts;

namespace Hays.Domain.Entities
{
    public class IncomeDefinition : BaseEntitiy
    {
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public ICollection<Income> Incomes { get; set; } = default!;
    }
}
