using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceTypesByEmployeeGroup
{
    public class GetAttendanceTypesByEmployeeGroupQueryHandler
        : IRequestHandler<GetAttendanceTypesByEmployeeGroupQuery, List<GetAttendanceTypesByEmployeeGroupQueryResult>>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public GetAttendanceTypesByEmployeeGroupQueryHandler(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task<List<GetAttendanceTypesByEmployeeGroupQueryResult>> Handle(
            GetAttendanceTypesByEmployeeGroupQuery request,
            CancellationToken cancellationToken)
        {
            var attendanceTypes = await _attendanceTypeRepository.GetByEmployeeGroupIdAsync(request.EmployeeGroupId, cancellationToken);

            return attendanceTypes
                .Select(at => new GetAttendanceTypesByEmployeeGroupQueryResult(
                    at.Id,
                    at.Name,
                    at.Abbreviation,
                    at.IsPresent,
                    at.Color))
                .ToList();
        }
    }
}
