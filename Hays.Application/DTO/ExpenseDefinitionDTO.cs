namespace Hays.Application.DTO
{
    public class ExpenseDefinitionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
