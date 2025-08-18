using Attendance.Domain.Events.AttendanceTypes;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Entities
{
    public class AttendanceType : BaseEntity<Guid>
    {
        public Guid EmployeeGroupId { get; private set; }
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public bool IsPresent { get; private set; }
        public string Color { get; private set; } // z.B. Hex-Code für die UI

        private AttendanceType() { }

        public AttendanceType(Guid employeeGroupId, string name, string abbreviation, bool isPresent, string color)
        {
            Id = Guid.NewGuid();
            EmployeeGroupId = employeeGroupId;
            Name = name;
            Abbreviation = abbreviation;
            IsPresent = isPresent;
            Color = color;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new AttendanceTypeCreatedEvent(this));
        }

        public void Update(string? name, string? abbreviation, bool? isPresent, string? color)
        {
            bool hasChanges = false;
            if (name is not null && !string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                hasChanges = true;
            }
            if (abbreviation is not null && !string.IsNullOrWhiteSpace(abbreviation) && Abbreviation != abbreviation)
            {
                Abbreviation = abbreviation;
                hasChanges = true;
            }
            if (isPresent.HasValue && IsPresent != isPresent.Value)
            {
                IsPresent = isPresent.Value;
                hasChanges = true;
            }
            if (color is not null && !string.IsNullOrWhiteSpace(color) && Color != color)
            {
                Color = color;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new AttendanceTypeUpdatedEvent(this));
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new AttendanceTypeDeletedEvent(this));
        }
    }
}
