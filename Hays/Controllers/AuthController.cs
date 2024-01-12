using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : AppControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("signin")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody] LoginCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("tokenactive")]
        [Authorize]
        public async Task<ActionResult> Tokenactive()
        {
            return Ok();
        }
    }
}
