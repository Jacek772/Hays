using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("/api/budgets")]
    [Authorize]
    public class BudgetsController : AppControllerBase
    {
        public BudgetsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<BudgetDTO>> GetBudgets([FromQuery] GetUserBudgetsQuery query)
        {
            if(query.UserId is null)
            {
                query.UserId = GetCurrentUserId();
            }
            return await _mediator.Send(query);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBudget([FromBody] UpdateBudgetCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{budgetId}")]
        public async Task<ActionResult> DeleteBudget(int budgetId)
        {
            await _mediator.Send(new DeleteBudgetCommand { BudgetId = budgetId });
            return Ok();
        }
    }
}
