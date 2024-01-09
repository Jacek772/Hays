using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserIncomesQueryHandler : IRequestHandler<GetUserIncomesQuery, List<IncomeDTO>>
    {
        private readonly IIncomesService _incomesService;
        private readonly IMapper _mapper;

        public GetUserIncomesQueryHandler(IIncomesService incomesService, IMapper mapper)
        {
            _incomesService = incomesService;
            _mapper = mapper;
        }

        public async Task<List<IncomeDTO>> Handle(GetUserIncomesQuery request, CancellationToken cancellationToken)
        {
            List<Income> incomes = await _incomesService.GetIncomesAsync(request);
            return _mapper.Map<List<Income>, List<IncomeDTO>>(incomes);
        }
    }
}
