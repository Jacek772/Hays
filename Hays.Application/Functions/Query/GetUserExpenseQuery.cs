using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetUserExpenseQuery : IRequest<ExpenseDTO>
    {
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
    }
}
