using Attendance.Domain.Events.AttendanceEntries;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Entities
{
    public class AttendanceEntry : BaseEntity<Guid>
    {
        public Guid EmployeeId { get; private set; }
        public DateTime Date { get; private set; }
        public Guid AttendanceTypeId { get; private set; }
        public string? Note { get; private set; }

        public AttendanceType AttendanceType { get; private set; }

        private AttendanceEntry() { }

        public AttendanceEntry(Guid employeeId, DateTime date, Guid attendanceTypeId, string? note = null)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            Date = date.Date;
            AttendanceTypeId = attendanceTypeId;
            Note = note;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new AttendanceEntryCreatedEvent(this));
        }

        public void Update(Guid attendanceTypeId, string? note)
        {
            bool hasChanges = false;
            if (AttendanceTypeId != attendanceTypeId)
            {
                AttendanceTypeId = attendanceTypeId;
                hasChanges = true;
            }
            if (Note != note)
            {
                Note = note;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new AttendanceEntryUpdatedEvent(this));
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new AttendanceEntryDeletedEvent(this));
        }
    }
}
