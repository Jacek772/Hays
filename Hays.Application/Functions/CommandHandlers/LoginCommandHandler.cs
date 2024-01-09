using Hays.Application.DTO;
using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDTO>
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IUsersService usersService,
            IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
        }

        public async Task<LoginDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User user = await _usersService.GetUserAsync(request.Email);
            if (user is null)
            {
                throw new BadRequestException("Bad user login or password");
            }

            string token = _authService.GenerateTokenAsync(user, request.Password);
            return new LoginDTO
            {
                Token = token
            };
        }
    }
}
