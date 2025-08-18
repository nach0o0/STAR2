using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntryByDate
{
    public class GetAttendanceEntryByDateQueryHandler
        : IRequestHandler<GetAttendanceEntryByDateQuery, GetAttendanceEntryByDateQueryResult?>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public GetAttendanceEntryByDateQueryHandler(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<GetAttendanceEntryByDateQueryResult?> Handle(
            GetAttendanceEntryByDateQuery request,
            CancellationToken cancellationToken)
        {
            var entry = await _entryRepository.GetByEmployeeAndDateAsync(request.EmployeeId, request.Date, cancellationToken);

            if (entry is null)
            {
                return null;
            }

            // Der .Include() im Repository stellt sicher, dass AttendanceType hier geladen ist.
            return new GetAttendanceEntryByDateQueryResult(
                entry.Id,
                entry.Date,
                entry.AttendanceTypeId,
                entry.AttendanceType.Name,
                entry.AttendanceType.Color,
                entry.Note
            );
        }
    }
}
