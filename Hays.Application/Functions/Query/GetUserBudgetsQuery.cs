using Hays.Application.DTO;
using MediatR;
using static Hays.Domain.Entities.Budget;

namespace Hays.Application.Functions.Query
{
    public class GetUserBudgetsQuery : IRequest<List<BudgetDTO>>
    {
        public int? UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public BudgetType? BudgetType { get; set; }
    }
}
