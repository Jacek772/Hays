using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetSpendingGoalsQueryHandler : IRequestHandler<GetSpendingGoalsQuery, List<SpendingGoalDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ISpendingGoalsService _spendingGoalsService;

        public GetSpendingGoalsQueryHandler(IMapper mapper, ISpendingGoalsService spendingGoalsService)
        {
            _mapper = mapper;
            _spendingGoalsService = spendingGoalsService;
        }

        public async Task<List<SpendingGoalDTO>> Handle(GetSpendingGoalsQuery request, CancellationToken cancellationToken)
        {
            List<SpendingGoal> spendingGoals = await _spendingGoalsService.GetSpendingGoalsAsync(request);
            return _mapper.Map<List<SpendingGoal>, List<SpendingGoalDTO>>(spendingGoals);
        }
    }
}
