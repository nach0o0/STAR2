using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceEntry
{
    public class DeleteAttendanceEntryCommandHandler : IRequestHandler<DeleteAttendanceEntryCommand>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public DeleteAttendanceEntryCommandHandler(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task Handle(DeleteAttendanceEntryCommand command, CancellationToken cancellationToken)
        {
            var entry = await _entryRepository.GetByIdAsync(command.AttendanceEntryId, cancellationToken);
            if (entry is null)
            {
                // Sollte durch den Authorizer abgedeckt sein, aber als Sicherheitsnetz.
                throw new NotFoundException(nameof(AttendanceEntry), command.AttendanceEntryId);
            }

            entry.PrepareForDeletion();
            _entryRepository.Delete(entry);
        }
    }
}
