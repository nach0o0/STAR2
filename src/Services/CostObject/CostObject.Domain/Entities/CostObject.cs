using CostObject.Domain.Enums;
using CostObject.Domain.Events.CostObjects;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class CostObject : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public Guid? ParentCostObjectId { get; private set; }
        public Guid HierarchyLevelId { get; private set; }
        public Guid? LabelId { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }
        public ApprovalStatus ApprovalStatus { get; private set; }

        private CostObject() { }

        // Der Konstruktor initialisiert die Kostenstelle und löst das Event aus
        public CostObject(string name, Guid employeeGroupId, Guid? parentCostObjectId, Guid hierarchyLevelId, Guid? labelId, DateTime validFrom, bool isApprovedDirectly)
        {
            Id = Guid.NewGuid();
            Name = name;
            EmployeeGroupId = employeeGroupId;
            ParentCostObjectId = parentCostObjectId;
            HierarchyLevelId = hierarchyLevelId;
            LabelId = labelId;
            ValidFrom = validFrom;
            ApprovalStatus = isApprovedDirectly ? ApprovalStatus.Approved : ApprovalStatus.Pending;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new CostObjectCreatedEvent(this));
        }

        public void Update(string? name, Guid? parentCostObjectId, Guid? hierarchyLevelId, Guid? labelId)
        {
            bool hasChanges = false;

            if (name is not null && !string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                hasChanges = true;
            }
            if (parentCostObjectId.HasValue && ParentCostObjectId != parentCostObjectId)
            {
                ParentCostObjectId = parentCostObjectId;
                hasChanges = true;
            }
            if (hierarchyLevelId.HasValue && HierarchyLevelId != hierarchyLevelId)
            {
                HierarchyLevelId = hierarchyLevelId.Value;
                hasChanges = true;
            }
            if (labelId.HasValue && LabelId != labelId)
            {
                LabelId = labelId;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new CostObjectUpdatedEvent(this));
            }
        }

        public void Deactivate(DateTime validToDate)
        {
            if (validToDate.Date < ValidFrom.Date)
            {
                throw new InvalidOperationException("ValidTo cannot be before ValidFrom.");
            }
            ValidTo = validToDate.Date;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new CostObjectDeactivatedEvent(this));
        }
    }
}
