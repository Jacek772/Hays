using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class DeleteExpenseDefinitionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
