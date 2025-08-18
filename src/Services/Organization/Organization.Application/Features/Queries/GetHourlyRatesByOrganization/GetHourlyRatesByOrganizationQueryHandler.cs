using MediatR;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetHourlyRatesByOrganization
{
    public class GetHourlyRatesByOrganizationQueryHandler
        : IRequestHandler<GetHourlyRatesByOrganizationQuery, List<GetHourlyRatesByOrganizationQueryResult>>
    {
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public GetHourlyRatesByOrganizationQueryHandler(IHourlyRateRepository hourlyRateRepository)
        {
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task<List<GetHourlyRatesByOrganizationQueryResult>> Handle(
            GetHourlyRatesByOrganizationQuery request,
            CancellationToken cancellationToken)
        {
            var hourlyRates = await _hourlyRateRepository.GetByOrganizationIdAsync(request.OrganizationId, cancellationToken);

            return hourlyRates
                .Select(hr => new GetHourlyRatesByOrganizationQueryResult(
                    hr.Id,
                    hr.Name,
                    hr.Rate,
                    hr.ValidFrom,
                    hr.ValidTo,
                    hr.Description))
                .ToList();
        }
    }
}
