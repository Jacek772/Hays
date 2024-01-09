using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserExpenseQueryHandler : IRequestHandler<GetUserExpenseQuery, ExpenseDTO>
    {
        private readonly IMapper _mapper;
        private readonly IExpensesService _expensesService;

        public GetUserExpenseQueryHandler(IExpensesService expensesService, IMapper mapper)
        {
            _expensesService = expensesService;
            _mapper = mapper;
        }

        public async Task<ExpenseDTO> Handle(GetUserExpenseQuery request, CancellationToken cancellationToken)
        {
            Expense expense = await _expensesService.GetExpenseAsync(request.UserId, request.ExpenseId);
            return _mapper.Map<Expense, ExpenseDTO>(expense);
        }
    }
}
