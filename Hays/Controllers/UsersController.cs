using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hays.Controllers
{
    [Route("/api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AppControllerBase
    {
        private readonly IMapper _mapper;

        public UsersController(IMediator mediator, IMapper mapper) : base(mediator)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<UserDTO>> GetUsers()
        {
            return await _mediator.Send(new GetUsersQuery());
        }

        [HttpGet("one/{userId}")]
        public async Task<UserDTO> GetUser(int userId)
        {
            return await _mediator.Send(new GetUserQuery { UserId = userId });
        }

        [HttpGet("current")]
        public async Task<UserDTO> GetCurrentUser()
        {
            return await _mediator.Send(new GetUserQuery { UserId = GetCurrentUserId() });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCurrentUser([FromBody] UpdateUserCommand command)
        {
            command.UserId = GetCurrentUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("one/{userId}")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] UpdateUserCommand command)
        {
            command.UserId = userId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCurrentUser()
        {
            await _mediator.Send(new DeleteUserCommand { UserId = GetCurrentUserId() });
            return Ok();
        }

        [HttpDelete("one/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            await _mediator.Send(new DeleteUserCommand { UserId = userId });
            return Ok();
        }

        private int GetCurrentUserId()
            => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
