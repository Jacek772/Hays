using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserExpensesQueryHandler : IRequestHandler<GetUserExpensesQuery, List<ExpenseDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IExpensesService _expensesService;

        public GetUserExpensesQueryHandler(IExpensesService expensesService, IMapper mapper)
        {
            _expensesService = expensesService;
            _mapper = mapper;
        }

        public async Task<List<ExpenseDTO>> Handle(GetUserExpensesQuery request, CancellationToken cancellationToken)
        {
            List<Expense> expenses = await _expensesService.GetExpensesAsync(request);
            return _mapper.Map<List<Expense>, List<ExpenseDTO>>(expenses);
        }
    }
}
