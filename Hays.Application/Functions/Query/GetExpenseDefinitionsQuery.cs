using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetExpenseDefinitionsQuery : IRequest<List<ExpenseDefinitionDTO>>
    {
    }
}
