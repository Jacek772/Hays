using Hays.Application.DTO;
using MediatR;

namespace Hays.Application.Functions.Query
{
    public class GetUserQuery : IRequest<UserDTO>
    {
        public int UserId { get; set; }
    }
}
