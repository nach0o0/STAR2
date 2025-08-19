using Planning.Domain.Events.PlanningEntries;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Domain.Entities
{
    public class PlanningEntry : BaseEntity<Guid>
    {
        public Guid EmployeeGroupId { get; private set; }
        public Guid EmployeeId { get; private set; }
        public Guid CostObjectId { get; private set; }
        public decimal PlannedHours { get; private set; }
        public DateTime PlanningPeriodStart { get; private set; }
        public DateTime PlanningPeriodEnd { get; private set; }
        public Guid PlannerId { get; private set; }
        public string? Notes { get; private set; }

        private PlanningEntry() { }

        public PlanningEntry(
            Guid employeeGroupId,
            Guid employeeId,
            Guid costObjectId,
            decimal plannedHours,
            DateTime planningPeriodStart,
            DateTime planningPeriodEnd,
            Guid plannerId,
            string? notes)
        {
            if (planningPeriodStart.Date > planningPeriodEnd.Date)
            {
                throw new ArgumentException("Start date cannot be after end date.");
            }

            Id = Guid.NewGuid();
            EmployeeGroupId = employeeGroupId;
            EmployeeId = employeeId;
            CostObjectId = costObjectId;
            PlannedHours = plannedHours;
            PlanningPeriodStart = planningPeriodStart.Date;
            PlanningPeriodEnd = planningPeriodEnd.Date;
            PlannerId = plannerId;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new PlanningEntryCreatedEvent(this));
        }

        public void Update(decimal newPlannedHours, string? newNotes)
        {
            PlannedHours = newPlannedHours;
            Notes = newNotes;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new PlanningEntryUpdatedEvent(this));
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new PlanningEntryDeletedEvent(this));
        }
    }
}
