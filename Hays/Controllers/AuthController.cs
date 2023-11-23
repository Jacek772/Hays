using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            LoginCommand loginCommand = new LoginCommand();
            return await _mediator.Send(loginCommand);
        }
    }
}
