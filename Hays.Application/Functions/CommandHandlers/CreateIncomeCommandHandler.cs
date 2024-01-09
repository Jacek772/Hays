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
    public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IIncomesService _incomesService;
        private readonly IBudgetsService _budgetsService;
        private readonly IUsersService _usersService;

        public CreateIncomeCommandHandler(IMapper mapper,
            IIncomesService incomesService, IBudgetsService budgetsService, IUsersService usersService)
        {
            _mapper = mapper;
            _incomesService = incomesService;
            _budgetsService = budgetsService;
            _usersService = usersService;
        }

        public async Task<Unit> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            Income income = _mapper.Map<CreateIncomeCommand, Income>(request);
            User user = await _usersService.GetUserAsync(request.UserId);
            income.BudgetId = await _budgetsService.GetBudgetIdForIncomeAsync(user, income);
            await _incomesService.CreateIncomeAsync(income);
            return Unit.Value;
        }
    }
}
