using AutoMapper;
using Hays.Application.DTO;
using Hays.Application.Exceptions;
using Hays.Application.Functions.Query;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using MediatR;

namespace Hays.Application.Functions.QueryHandlers
{
    public class GetUserIncomeQueryHandler : IRequestHandler<GetUserIncomeQuery, IncomeDTO>
    {
        private readonly IIncomesService _incomesService;
        private readonly IMapper _mapper;

        public GetUserIncomeQueryHandler(IIncomesService incomesService, IMapper mapper)
        {
            _incomesService = incomesService;
            _mapper = mapper;
        }

        public async Task<IncomeDTO> Handle(GetUserIncomeQuery request, CancellationToken cancellationToken)
        {
            Income income = await _incomesService.GetIncomeAsync(request.UserId, request.IncomeId);
            if (income is null)
            {
                throw new NotFoundException("Income does not exists");
            }

            return _mapper.Map<Income, IncomeDTO>(income);
        }
    }
}
