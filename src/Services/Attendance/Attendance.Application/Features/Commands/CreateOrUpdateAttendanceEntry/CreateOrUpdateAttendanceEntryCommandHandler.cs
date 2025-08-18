using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateOrUpdateAttendanceEntry
{
    public class CreateOrUpdateAttendanceEntryCommandHandler : IRequestHandler<CreateOrUpdateAttendanceEntryCommand, Guid>
    {
        private readonly IAttendanceEntryRepository _entryRepository;

        public CreateOrUpdateAttendanceEntryCommandHandler(IAttendanceEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<Guid> Handle(CreateOrUpdateAttendanceEntryCommand command, CancellationToken cancellationToken)
        {
            // Prüfen, ob bereits ein Eintrag für diesen Tag existiert
            var existingEntry = await _entryRepository.GetByEmployeeAndDateAsync(command.EmployeeId, command.Date, cancellationToken);

            if (existingEntry != null)
            {
                // Eintrag aktualisieren
                existingEntry.Update(command.AttendanceTypeId, command.Note);
                return existingEntry.Id;
            }
            else
            {
                // Neuen Eintrag erstellen
                var newEntry = new AttendanceEntry(
                    command.EmployeeId,
                    command.Date,
                    command.AttendanceTypeId,
                    command.Note
                );
                await _entryRepository.AddAsync(newEntry, cancellationToken);
                return newEntry.Id;
            }
        }
    }
}
