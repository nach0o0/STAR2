using Shared.Domain.Abstractions;
using TimeTracking.Domain.Events.TimeEntries;

namespace TimeTracking.Domain.Entities
{
    public class TimeEntry : BaseEntity<Guid>
    {
        public Guid? EmployeeId { get; private set; }
        public Guid? CostObjectId { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public DateTime EntryDate { get; private set; }
        public decimal Hours { get; private set; }
        public decimal HourlyRate { get; private set; }
        public string? Description { get; private set; }
        public bool IsAnonymized { get; private set; }
        public Guid? AccessKey { get; private set; }

        private TimeEntry() { }

        public TimeEntry(
            Guid? employeeId,
            Guid? costObjectId,
            Guid employeeGroupId,
            DateTime entryDate,
            decimal hours,
            decimal hourlyRate,
            string? description,
            bool createAnonymously = false)
        {
            Id = Guid.NewGuid();
            CostObjectId = costObjectId;
            EmployeeGroupId = employeeGroupId;
            EntryDate = entryDate.Date;
            Hours = hours;
            HourlyRate = hourlyRate;
            Description = description;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new TimeEntryCreatedEvent(this));

            if (createAnonymously)
            {
                EmployeeId = null;
                IsAnonymized = true;
                AccessKey = Guid.NewGuid();

                AddDomainEvent(new TimeEntryAnonymizedEvent(this, AccessKey.Value));
            }
            else
            {
                if (!employeeId.HasValue)
                {
                    throw new ArgumentNullException(nameof(employeeId), "EmployeeId must be provided for non-anonymous time entries.");
                }
                EmployeeId = employeeId;
                IsAnonymized = false;
                AccessKey = null;
            }
        }

        // Angepasste Update-Methode
        public void Update(DateTime? newEntryDate, Guid? newCostObjectId, decimal? newHours, string? newDescription)
        {
            bool hasChanges = false;
            if (newEntryDate.HasValue && EntryDate.Date != newEntryDate.Value.Date)
            {
                EntryDate = newEntryDate.Value.Date;
                hasChanges = true;
            }
            if (newCostObjectId.HasValue && CostObjectId != newCostObjectId)
            {
                CostObjectId = newCostObjectId.Value;
                hasChanges = true;
            }
            if (newHours.HasValue && Hours != newHours.Value)
            {
                Hours = newHours.Value;
                hasChanges = true;
            }
            if (Description != newDescription)
            {
                Description = newDescription;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new TimeEntryUpdatedEvent(this));
            }
        }

        // Neue Methode
        public void PrepareForDeletion()
        {
            AddDomainEvent(new TimeEntryDeletedEvent(this));
        }

        public void Anonymize()
        {
            if (IsAnonymized) return;

            EmployeeId = null;
            IsAnonymized = true;
            AccessKey = Guid.NewGuid();
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new TimeEntryAnonymizedEvent(this, AccessKey.Value));
        }
    }
}
