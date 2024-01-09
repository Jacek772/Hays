using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Exceptions;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            User user = await _usersService.GetUserAsync(request.UserId);
            if (user is null)
            {
                throw new BadRequestException("User not exists");
            }

            return _mapper.Map<UserDTO>(user);
        }
    }
}
