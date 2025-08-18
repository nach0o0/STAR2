using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetPublicHolidaysByDateRange
{
    public class GetPublicHolidaysByDateRangeQueryHandler
        : IRequestHandler<GetPublicHolidaysByDateRangeQuery, List<GetPublicHolidaysByDateRangeQueryResult>>
    {
        private readonly IPublicHolidayRepository _publicHolidayRepository;

        public GetPublicHolidaysByDateRangeQueryHandler(IPublicHolidayRepository publicHolidayRepository)
        {
            _publicHolidayRepository = publicHolidayRepository;
        }

        public async Task<List<GetPublicHolidaysByDateRangeQueryResult>> Handle(
            GetPublicHolidaysByDateRangeQuery request,
            CancellationToken cancellationToken)
        {
            var holidays = await _publicHolidayRepository.GetByDateRangeAndGroupsAsync(
                request.EmployeeGroupIds,
                request.StartDate,
                request.EndDate,
                cancellationToken
            );

            return holidays
                .Select(ph => new GetPublicHolidaysByDateRangeQueryResult(
                    ph.Id,
                    ph.Name,
                    ph.Date,
                    ph.EmployeeGroupId))
                .ToList();
        }
    }
}
