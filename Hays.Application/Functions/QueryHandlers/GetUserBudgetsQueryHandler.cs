using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserBudgetsQueryHandler : IRequestHandler<GetUserBudgetsQuery, List<BudgetDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IBudgetsService _budgetsService;
        private readonly IIncomesService _incomesService;
        private readonly IExpensesService _expensesService;

        public GetUserBudgetsQueryHandler(IBudgetsService budgetsService, IIncomesService incomesService,
            IExpensesService expensesService, IMapper mapper)
        {
            _budgetsService = budgetsService;
            _incomesService = incomesService;
            _expensesService = expensesService;
            _mapper = mapper;
        }

        public async Task<List<BudgetDTO>> Handle(GetUserBudgetsQuery request, CancellationToken cancellationToken)
        {
            List<Budget> budgets = await _budgetsService.GetBudgetsAsync(request);
            List<BudgetDTO> budgetsDTO = _mapper.Map<List<Budget>, List<BudgetDTO>>(budgets);
            return await CalculateBudgetsIncomeAndExpense(budgets, budgetsDTO);
        }

        private async Task<List<BudgetDTO>> CalculateBudgetsIncomeAndExpense(List<Budget> budgets, List<BudgetDTO> budgetsDTO)
        {
            foreach (BudgetDTO budgetDTO in budgetsDTO)
            {
                List<Income> incomes = new List<Income>();
                List<Expense> expenses = new List<Expense>();
                if (budgetDTO.Type == BudgetDTO.BudgetType.Yearly)
                {
                    incomes = await _incomesService.GetIncomesByYearlyBudgetIdAsync(budgetDTO.Id);
                    expenses = await _expensesService.GetExpensesByYearlyBudgetIdAsync(budgetDTO.Id);
                }
                else
                {
                    incomes = await _incomesService.GetIncomesByMonthlyBudgetIdAsync(budgetDTO.Id);
                    expenses = await _expensesService.GetExpensesByMonthlyBudgetIdAsync(budgetDTO.Id);
                }

                budgetDTO.Income = incomes.Sum(x => x.Amount);
                budgetDTO.Expense = expenses.Sum(x => x.Amount);

                if(budgetDTO.Type == BudgetDTO.BudgetType.Yearly)
                {
                    Budget budget = budgets.FirstOrDefault(x => x.Id == budgetDTO.Id);
                    budgetDTO.BudgetValue = budget.Children.Sum(x => x.BudgetValue);
                }
            }
            return budgetsDTO;
        }
    }
}
