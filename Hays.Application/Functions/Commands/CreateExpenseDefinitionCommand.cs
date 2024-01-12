using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class CreateExpenseDefinitionCommand : IRequest<Unit>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}