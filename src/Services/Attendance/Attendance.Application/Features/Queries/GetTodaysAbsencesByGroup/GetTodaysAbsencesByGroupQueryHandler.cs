using Attendance.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetTodaysAbsencesByGroup
{
    public class GetTodaysAbsencesByGroupQueryHandler
        : IRequestHandler<GetTodaysAbsencesByGroupQuery, List<GetTodaysAbsencesByGroupQueryResult>>
    {
        private readonly IAttendanceEntryRepository _entryRepository;
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public GetTodaysAbsencesByGroupQueryHandler(
            IAttendanceEntryRepository entryRepository,
            IOrganizationServiceClient organizationServiceClient)
        {
            _entryRepository = entryRepository;
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<List<GetTodaysAbsencesByGroupQueryResult>> Handle(
            GetTodaysAbsencesByGroupQuery request,
            CancellationToken cancellationToken)
        {
            var employeesInGroup = await _organizationServiceClient.GetEmployeesByEmployeeGroupAsync(request.EmployeeGroupId, cancellationToken);
            var employeeIds = employeesInGroup.Select(e => e.Id).ToList();

            if (!employeeIds.Any())
            {
                return new List<GetTodaysAbsencesByGroupQueryResult>();
            }

            var absenceEntries = await _entryRepository.GetAbsencesForEmployeesByDateAsync(
                employeeIds,
                request.Date,
                cancellationToken
            );

            return absenceEntries
                .Select(e => new GetTodaysAbsencesByGroupQueryResult(
                    e.EmployeeId,
                    e.AttendanceType.Name,
                    e.Note))
                .ToList();
        }
    }
}
