using Attendance.Domain.Events.PublicHolidays;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Entities
{
    public class PublicHoliday : BaseEntity<Guid>
    {
        public DateTime Date { get; private set; }
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }

        private PublicHoliday() { }

        public PublicHoliday(DateTime date, string name, Guid employeeGroupId)
        {
            Id = Guid.NewGuid();
            Date = date.Date;
            Name = name;
            EmployeeGroupId = employeeGroupId;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new PublicHolidayCreatedEvent(this));
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new PublicHolidayDeletedEvent(this));
        }
    }
}
