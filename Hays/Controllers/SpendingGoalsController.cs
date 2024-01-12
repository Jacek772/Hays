using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("api/spendinggoals")]
    [Authorize]
    public class SpendingGoalsController : AppControllerBase
    {
        public SpendingGoalsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<SpendingGoalDTO>> GetSpendingGoals([FromQuery] GetSpendingGoalsQuery query)
        {
            if (query.UserId is null)
            {
                query.UserId = GetCurrentUserId();
            }

            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSpendingGoal([FromBody] CreateSpendingGoalCommand command)
        {
            command.UserId = GetCurrentUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSpendingGoal(int id)
        {
            await _mediator.Send(new DeleteSpendingGoalCommand { Id = id });
            return Ok();
        }
    }
}
