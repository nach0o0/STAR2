using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateAttendanceType
{
    public class CreateAttendanceTypeCommandHandler : IRequestHandler<CreateAttendanceTypeCommand, Guid>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public CreateAttendanceTypeCommandHandler(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task<Guid> Handle(CreateAttendanceTypeCommand command, CancellationToken cancellationToken)
        {
            var attendanceType = new AttendanceType(
                command.EmployeeGroupId,
                command.Name,
                command.Abbreviation,
                command.IsPresent,
                command.Color
            );

            await _attendanceTypeRepository.AddAsync(attendanceType, cancellationToken);

            return attendanceType.Id;
        }
    }
}
