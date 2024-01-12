using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteIncomeDefinitionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
