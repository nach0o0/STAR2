using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceSummaryForEmployee
{
    public class GetAttendanceSummaryForEmployeeQueryHandler
        : IRequestHandler<GetAttendanceSummaryForEmployeeQuery, List<GetAttendanceSummaryForEmployeeQueryResult>>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public GetAttendanceSummaryForEmployeeQueryHandler(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<List<GetAttendanceSummaryForEmployeeQueryResult>> Handle(
            GetAttendanceSummaryForEmployeeQuery request,
            CancellationToken cancellationToken)
        {
            var summaryDictionary = await _entryRepository.GetSummaryByEmployeeAndDateRangeAsync(
                request.EmployeeId,
                request.StartDate,
                request.EndDate,
                cancellationToken
            );

            return summaryDictionary
                .Select(kvp => new GetAttendanceSummaryForEmployeeQueryResult(
                    kvp.Key.Id,
                    kvp.Key.Name,
                    kvp.Key.Color,
                    kvp.Value))
                .ToList();
        }
    }
}
