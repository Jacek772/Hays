using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hays.Controllers.Abstracts
{
    [ApiController]
    public abstract class AppControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        public AppControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected int GetCurrentUserId()
        {
            if(int.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                return id;
            }
            return 0;
        }
    }
}
