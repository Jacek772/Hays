using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetUserExpensesQuery : IRequest<List<ExpenseDTO>>
    {
        public int UserId { get; set; }
        public string? SearchPhrase { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
