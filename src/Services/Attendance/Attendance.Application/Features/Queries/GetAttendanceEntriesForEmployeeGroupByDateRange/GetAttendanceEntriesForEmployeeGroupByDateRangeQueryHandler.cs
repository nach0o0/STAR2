using Attendance.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeGroupByDateRange
{
    public class GetAttendanceEntriesForEmployeeGroupByDateRangeQueryHandler
        : IRequestHandler<GetAttendanceEntriesForEmployeeGroupByDateRangeQuery, List<GetAttendanceEntriesForEmployeeGroupByDateRangeQueryResult>>
    {
        private readonly IAttendanceEntryRepository _entryRepository;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetAttendanceEntriesForEmployeeGroupByDateRangeQueryHandler(
            IAttendanceEntryRepository entryRepository,
            IOrganizationServiceClient organizationServiceClient)
        {
            _entryRepository = entryRepository;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<List<GetAttendanceEntriesForEmployeeGroupByDateRangeQueryResult>> Handle(
            GetAttendanceEntriesForEmployeeGroupByDateRangeQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Hole alle Mitarbeiter-IDs aus der Gruppe vom Organization Service
            var employeesInGroup = await _organizationServiceClient.GetEmployeesByEmployeeGroupAsync(request.EmployeeGroupId, cancellationToken);
            var employeeIds = employeesInGroup.Select(e => e.Id).ToList();

            if (!employeeIds.Any())
            {
                return new List<GetAttendanceEntriesForEmployeeGroupByDateRangeQueryResult>();
            }

            // 2. Hole die Anwesenheitseinträge für diese Mitarbeiter im Datumsbereich
            var entries = await _entryRepository.GetForEmployeesByDateRangeAsync(
                employeeIds,
                request.StartDate,
                request.EndDate,
                cancellationToken
            );

            // 3. Mappe die Ergebnisse
            return entries
                .Select(e => new GetAttendanceEntriesForEmployeeGroupByDateRangeQueryResult(
                    e.Id,
                    e.EmployeeId,
                    e.Date,
                    e.AttendanceTypeId,
                    e.AttendanceType.Name,
                    e.AttendanceType.Color,
                    e.Note))
                .ToList();
        }
    }
}
