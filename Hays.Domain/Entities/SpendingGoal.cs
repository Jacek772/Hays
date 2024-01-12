using Hays.Domain.Entities.Abstracts;

namespace Hays.Domain.Entities
{
    public class SpendingGoal : BaseEntitiy
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
