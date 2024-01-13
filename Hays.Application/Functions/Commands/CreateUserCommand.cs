using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class CreateUserCommand : IRequest<Unit>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
    }
}
