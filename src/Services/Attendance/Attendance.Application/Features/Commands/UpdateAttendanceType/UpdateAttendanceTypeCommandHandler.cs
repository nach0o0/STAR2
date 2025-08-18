using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateAttendanceType
{
    public class UpdateAttendanceTypeCommandHandler : IRequestHandler<UpdateAttendanceTypeCommand>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public UpdateAttendanceTypeCommandHandler(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task Handle(UpdateAttendanceTypeCommand command, CancellationToken cancellationToken)
        {
            var attendanceType = await _attendanceTypeRepository.GetByIdAsync(command.AttendanceTypeId, cancellationToken);
            if (attendanceType is null)
            {
                throw new NotFoundException(nameof(AttendanceType), command.AttendanceTypeId);
            }

            attendanceType.Update(
                command.Name,
                command.Abbreviation,
                command.IsPresent,
                command.Color
            );
        }
    }
}
