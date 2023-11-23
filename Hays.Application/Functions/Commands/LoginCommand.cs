using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Commands
{
    public class LoginCommand : IRequest<LoginDTO>
    {
        public string Login { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
