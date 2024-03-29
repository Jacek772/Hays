﻿using Hays.Domain.Entities.Abstracts;

namespace Hays.Domain.Entities
{
    public class User : BaseEntitiy
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public byte[] Salt { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Surname { get; set; }

        public ICollection<Budget> Budgets { get; set; } = default!;
        public ICollection<SpendingGoal> SpendingGoals { get; set; } = default!;
    }
}
