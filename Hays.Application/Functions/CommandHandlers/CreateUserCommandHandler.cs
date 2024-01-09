using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly IUsersService _usersService;

        public CreateUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool exists = await _usersService.ExistsUserAsync(request.Login);
            if (exists)
            {
                throw new BadRequestException($"User with login {request.Login} exists!");
            }

            await _usersService.CreateUserAsync(request);
            return Unit.Value;
        }
    }
}
