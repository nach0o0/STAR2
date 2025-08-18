using Attendance.Domain.Events.WorkModels;
using Shared.Domain.Abstractions;

namespace Attendance.Domain.Entities
{
    public class WorkModel : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public decimal MondayHours { get; private set; }
        public decimal TuesdayHours { get; private set; }
        public decimal WednesdayHours { get; private set; }
        public decimal ThursdayHours { get; private set; }
        public decimal FridayHours { get; private set; }
        public decimal SaturdayHours { get; private set; }
        public decimal SundayHours { get; private set; }

        private WorkModel() { }

        public WorkModel(
            string name,
            Guid employeeGroupId,
            decimal mondayHours,
            decimal tuesdayHours,
            decimal wednesdayHours,
            decimal thursdayHours,
            decimal fridayHours,
            decimal saturdayHours,
            decimal sundayHours)
        {
            Id = Guid.NewGuid();
            Name = name;
            EmployeeGroupId = employeeGroupId;
            MondayHours = mondayHours;
            TuesdayHours = tuesdayHours;
            WednesdayHours = wednesdayHours;
            ThursdayHours = thursdayHours;
            FridayHours = fridayHours;
            SaturdayHours = saturdayHours;
            SundayHours = sundayHours;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string? name,
            decimal? mondayHours,
            decimal? tuesdayHours,
            decimal? wednesdayHours,
            decimal? thursdayHours,
            decimal? fridayHours,
            decimal? saturdayHours,
            decimal? sundayHours)
        {
            bool hasChanges = false;

            if (name is not null && !string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                hasChanges = true;
            }
            if (mondayHours.HasValue && MondayHours != mondayHours.Value) { MondayHours = mondayHours.Value; hasChanges = true; }
            if (tuesdayHours.HasValue && TuesdayHours != tuesdayHours.Value) { TuesdayHours = tuesdayHours.Value; hasChanges = true; }
            if (wednesdayHours.HasValue && WednesdayHours != wednesdayHours.Value) { WednesdayHours = wednesdayHours.Value; hasChanges = true; }
            if (thursdayHours.HasValue && ThursdayHours != thursdayHours.Value) { ThursdayHours = thursdayHours.Value; hasChanges = true; }
            if (fridayHours.HasValue && FridayHours != fridayHours.Value) { FridayHours = fridayHours.Value; hasChanges = true; }
            if (saturdayHours.HasValue && SaturdayHours != saturdayHours.Value) { SaturdayHours = saturdayHours.Value; hasChanges = true; }
            if (sundayHours.HasValue && SundayHours != sundayHours.Value) { SundayHours = sundayHours.Value; hasChanges = true; }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new WorkModelUpdatedEvent(this));
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new WorkModelDeletedEvent(this));
        }
    }
}
