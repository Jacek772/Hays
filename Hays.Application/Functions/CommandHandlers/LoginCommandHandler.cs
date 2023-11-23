using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDTO>
    {
        public Task<LoginDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
