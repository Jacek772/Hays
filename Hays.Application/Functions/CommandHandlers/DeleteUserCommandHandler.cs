using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUsersService _usersService;

        public DeleteUserCommandHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _usersService.DeleteUserAsync(request.UserId);
            return Unit.Value;
        }
    }
}
