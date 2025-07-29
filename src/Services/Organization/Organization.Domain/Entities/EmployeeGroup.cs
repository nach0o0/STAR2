using Organization.Domain.Events.EmployeeGroups;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities
{
    public class EmployeeGroup : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid LeadingOrganizationId { get; private set; }

        private EmployeeGroup() { }

        public EmployeeGroup(string name, Guid leadingOrganizationId)
        {
            Id = Guid.NewGuid();
            Name = name;
            LeadingOrganizationId = leadingOrganizationId;
            CreatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeGroupCreatedEvent(this));
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName) || Name == newName)
            {
                return;
            }

            Name = newName;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeGroupUpdatedEvent(this));
        }

        public void TransferToOrganization(Guid destinationOrganizationId)
        {
            if (LeadingOrganizationId == destinationOrganizationId)
            {
                return;
            }
            var sourceOrganizationId = LeadingOrganizationId;
            LeadingOrganizationId = destinationOrganizationId;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeGroupTransferredEvent(Id, sourceOrganizationId, destinationOrganizationId));
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new EmployeeGroupDeletedEvent(this));
        }
    }
}
