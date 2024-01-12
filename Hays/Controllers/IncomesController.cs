using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Commands;
using Hays.Application.Functions.Query;
using Hays.Controllers.Abstracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("/api/incomes")]
    [Authorize]
    public class IncomesController : AppControllerBase
    {
        public IncomesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<List<IncomeDTO>> GetIncomes([FromQuery] GetUserIncomesQuery query)
        {
            if(query.UserId is null)
            {
                query.UserId = GetCurrentUserId();
            }

            return await _mediator.Send(query);
        }

        [HttpGet("{incomeId}")]
        public async Task<IncomeDTO> GetIncome(int incomeId)
        {
            GetUserIncomeQuery getUserIncomeQuery = new GetUserIncomeQuery
            {
                IncomeId = incomeId,
                UserId = GetCurrentUserId()
            };

            return await _mediator.Send(getUserIncomeQuery);
        }

        [HttpPost]
        public async Task<ActionResult> CreateIncome([FromBody] CreateIncomeCommand command)
        {
            command.UserId = GetCurrentUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateIncome([FromBody] UpdateIncomeCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{incomeId}")]
        public async Task<ActionResult> DeleteIncome(int incomeId)
        {
            await _mediator.Send(new DeleteIncomeCommand { IncomeId = incomeId });
            return Ok();
        }
    }
}
