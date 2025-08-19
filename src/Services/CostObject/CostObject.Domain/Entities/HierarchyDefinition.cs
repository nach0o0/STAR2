using CostObject.Domain.Events.HierarchyDefinitions;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class HierarchyDefinition : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public Guid? RequiredBookingLevelId { get; private set; }

        private HierarchyDefinition() { }

        public HierarchyDefinition(string name, Guid employeeGroupId, Guid? requiredBookingLevelId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            EmployeeGroupId = employeeGroupId;
            RequiredBookingLevelId = requiredBookingLevelId;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new HierarchyDefinitionCreatedEvent(this));
        }

        public void Update(string? newName, Guid? newRequiredBookingLevelId)
        {
            bool hasChanges = false;
            if (newName is not null && !string.IsNullOrWhiteSpace(newName) && Name != newName)
            {
                Name = newName;
                hasChanges = true;
            }

            // Diese Logik erlaubt es auch, die RequiredBookingLevelId zu entfernen (indem man null übergibt)
            if (RequiredBookingLevelId != newRequiredBookingLevelId)
            {
                RequiredBookingLevelId = newRequiredBookingLevelId;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new HierarchyDefinitionUpdatedEvent(this));
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new HierarchyDefinitionDeletedEvent(this));
        }

        public void ClearRequiredBookingLevel()
        {
            if (RequiredBookingLevelId is null)
            {
                return;
            }
            RequiredBookingLevelId = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
