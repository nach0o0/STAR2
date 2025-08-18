using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetHourlyRateById
{
    public class GetHourlyRateByIdQueryHandler
        : IRequestHandler<GetHourlyRateByIdQuery, GetHourlyRateByIdQueryResult?>
    {
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public GetHourlyRateByIdQueryHandler(IHourlyRateRepository hourlyRateRepository)
        {
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task<GetHourlyRateByIdQueryResult?> Handle(
            GetHourlyRateByIdQuery request,
            CancellationToken cancellationToken)
        {
            var hourlyRate = await _hourlyRateRepository.GetByIdAsync(request.HourlyRateId, cancellationToken);

            if (hourlyRate is null)
            {
                return null;
            }

            return new GetHourlyRateByIdQueryResult(
                hourlyRate.Id,
                hourlyRate.Name,
                hourlyRate.Rate,
                hourlyRate.ValidFrom,
                hourlyRate.ValidTo,
                hourlyRate.Description,
                hourlyRate.OrganizationId
            );
        }
    }
}
