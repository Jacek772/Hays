using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("/api/expensedefinitions")]
    [Authorize]
    public class ExpenseDefinitionsController : AppControllerBase
    {
        public ExpenseDefinitionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<ExpenseDefinitionDTO>> GetExpenseDefinitions()
        {
            
            return await _mediator.Send(new GetExpenseDefinitionsQuery());
        }

        [HttpPost]
        public async Task<ActionResult> CreateExpenseDefinition([FromBody] CreateExpenseDefinitionCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpenseDefinition(int id)
        {
            await _mediator.Send(new DeleteExpenseDefinitionCommand { Id = id });
            return Ok();
        }
    }
}
