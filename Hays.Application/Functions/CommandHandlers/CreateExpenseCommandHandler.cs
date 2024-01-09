using AutoMapper;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.Functions.CommandHandlers
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IExpensesService _expensesService;
        private readonly IBudgetsService _budgetsService;
        private readonly IUsersService _usersService;

        public CreateExpenseCommandHandler(IMapper mapper,
            IExpensesService expensesService, IBudgetsService budgetsService, IUsersService usersService)
        {
            _mapper = mapper;
            _expensesService = expensesService;
            _budgetsService = budgetsService;
            _usersService = usersService;
        }

        public async Task<Unit> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            Expense expense = _mapper.Map<CreateExpenseCommand, Expense>(request);
            User user = await _usersService.GetUserAsync(request.UserId);
            expense.BudgetId = await _budgetsService.GetBudgetIdForExpenseAsync(user, expense);
            await _expensesService.CreateExpenseAsync(expense);
            return Unit.Value;
        }
    }
}
