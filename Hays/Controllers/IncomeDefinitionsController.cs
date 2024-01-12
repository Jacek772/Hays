using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("/api/incomedefinitions")]
    [Authorize]
    public class IncomeDefinitionsController : AppControllerBase
    {
        public IncomeDefinitionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<IncomeDefinitionDTO>> GetIncomeDefinitions()
        {
            return await _mediator.Send(new GetIncomeDefinitionsQuery());
        }

        [HttpPost]
        public async Task<ActionResult> CreateIncomeDefinition([FromBody] CreateIncomeDefinitionCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIncomeDefinition(int id)
        {
            await _mediator.Send(new DeleteIncomeDefinitionCommand { Id = id });
            return Ok();
        }
    }
}
