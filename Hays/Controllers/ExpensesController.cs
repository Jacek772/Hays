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
    [Route("/api/expenses")]
    [Authorize]
    public class ExpensesController : AppControllerBase
    {
        private readonly IMapper _mapper;

        public ExpensesController(IMediator mediator, IMapper mapper) : base(mediator)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<ExpenseDTO>> GetExpenses(string? searchPhrase, int? page, int? pageSize)
        {
            return await _mediator.Send(new GetUserExpensesQuery()
            {
                UserId = GetCurrentUserId(),
                SearchPhrase = searchPhrase,
                Page = page,
                PageSize = pageSize
            });
        }

        [HttpGet("{expenseId}")]
        public async Task<ExpenseDTO> GetExpense(int expenseId)
        {
            return await _mediator.Send(new GetUserExpenseQuery
            {
                ExpenseId = expenseId,
                UserId = GetCurrentUserId()
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateExpense([FromBody] CreateExpenseCommand command)
        {
            command.UserId = GetCurrentUserId();
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateExpense(int expenseId, [FromBody] UpdateExpenseCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{expenseId}")]
        public async Task<ActionResult> DeleteExpense(int expenseId)
        {
            await _mediator.Send(new DeleteExpenseCommand { ExpenseId = expenseId });
            return Ok();
        }
    }
}
