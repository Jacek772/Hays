using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly IUsersService _usersService;
        private readonly IBudgetsService _budgetsService;

        public CreateUserCommandHandler(IUsersService usersService, IBudgetsService budgetsService)
        {
            _usersService = usersService;
            _budgetsService = budgetsService;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            bool exists = await _usersService.ExistsUserAsync(request.Email);
            if (exists)
            {
                throw new BadRequestException($"User with email {request.Email} exists!");
            }

            User user = await _usersService.CreateUserAsync(request);
            await _budgetsService.InitBudgetsForUser(user);
            return Unit.Value;
        }
    }
}
