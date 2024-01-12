using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.DTO
{
    public class SpendingGoalDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = default!;
        public decimal Amount { get; set; }
    }
}
