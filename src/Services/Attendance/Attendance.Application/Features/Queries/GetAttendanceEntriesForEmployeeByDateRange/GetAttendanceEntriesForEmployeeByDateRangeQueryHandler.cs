using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeByDateRange
{
    public class GetAttendanceEntriesForEmployeeByDateRangeQueryHandler
        : IRequestHandler<GetAttendanceEntriesForEmployeeByDateRangeQuery, List<GetAttendanceEntriesForEmployeeByDateRangeQueryResult>>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public GetAttendanceEntriesForEmployeeByDateRangeQueryHandler(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<List<GetAttendanceEntriesForEmployeeByDateRangeQueryResult>> Handle(
            GetAttendanceEntriesForEmployeeByDateRangeQuery request,
            CancellationToken cancellationToken)
        {
            var entries = await _entryRepository.GetForEmployeeInGroupsByDateRangeAsync(
                request.EmployeeId,
                request.EmployeeGroupIds,
                request.StartDate,
                request.EndDate,
                cancellationToken
            );

            return entries
                .Select(e => new GetAttendanceEntriesForEmployeeByDateRangeQueryResult(
                    e.Id,
                    e.Date,
                    e.AttendanceTypeId,
                    e.AttendanceType.Name,  // Dank Include() im Repository verfügbar
                    e.AttendanceType.Color, // Dank Include() im Repository verfügbar
                    e.Note))
                .ToList();
        }
    }
}
