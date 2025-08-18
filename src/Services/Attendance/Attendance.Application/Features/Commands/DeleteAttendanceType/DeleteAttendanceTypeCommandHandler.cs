using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceType
{
    public class DeleteAttendanceTypeCommandHandler : IRequestHandler<DeleteAttendanceTypeCommand>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public DeleteAttendanceTypeCommandHandler(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task Handle(DeleteAttendanceTypeCommand command, CancellationToken cancellationToken)
        {
            var attendanceType = await _attendanceTypeRepository.GetByIdAsync(command.AttendanceTypeId, cancellationToken);
            if (attendanceType is null)
            {
                // Sollte durch den Authorizer abgedeckt sein, aber als Sicherheitsnetz.
                throw new NotFoundException(nameof(AttendanceType), command.AttendanceTypeId);
            }

            attendanceType.PrepareForDeletion();
            _attendanceTypeRepository.Delete(attendanceType);
        }
    }
}
