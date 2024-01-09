using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetUserIncomeQuery : IRequest<IncomeDTO>
    {
        public int IncomeId { get; set; }
        public int UserId { get; set; }
    }
}
