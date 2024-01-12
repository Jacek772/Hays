using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetIncomeDefinitionsQuery : IRequest<List<IncomeDefinitionDTO>>
    {
    }
}
