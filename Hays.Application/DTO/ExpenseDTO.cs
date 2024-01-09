namespace Hays.Application.DTO
{
    public class ExpenseDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public ExpenseDefinitionDTO Definition { get; set; } = default!;
    }
}
