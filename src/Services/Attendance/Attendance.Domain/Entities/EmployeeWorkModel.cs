using Attendance.Domain.Events.EmployeeWorkModels;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Entities
{
    public class EmployeeWorkModel : BaseEntity<Guid>
    {
        public Guid EmployeeId { get; private set; }
        public Guid WorkModelId { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        private EmployeeWorkModel() { }

        public EmployeeWorkModel(Guid employeeId, Guid workModelId, DateTime validFrom, DateTime? validTo = null)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            WorkModelId = workModelId;
            ValidFrom = validFrom;
            ValidTo = validTo;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new EmployeeWorkModelAssignedEvent(this));
        }

        public void EndAssignment(DateTime endDate)
        {
            if (endDate.Date < ValidFrom.Date)
            {
                throw new InvalidOperationException("End date cannot be before the start date.");
            }
            ValidTo = endDate.Date;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new EmployeeWorkModelUnassignedEvent(EmployeeId, WorkModelId));
        }

        public void Update(DateTime? validFrom, DateTime? validTo)
        {
            bool hasChanges = false;
            if (validFrom.HasValue && ValidFrom.Date != validFrom.Value.Date)
            {
                ValidFrom = validFrom.Value.Date;
                hasChanges = true;
            }
            if (ValidTo?.Date != validTo?.Date)
            {
                ValidTo = validTo?.Date;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new EmployeeWorkModelUpdatedEvent(this));
            }
        }
    }
}
