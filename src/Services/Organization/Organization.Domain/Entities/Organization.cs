using Organization.Domain.Events.Organizations;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities
{
    public class Organization : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public Guid? ParentOrganizationId { get; private set; }
        public Guid? DefaultEmployeeGroupId { get; private set; }

        private Organization() { }

        public Organization(string name, string abbreviation, Guid? parentOrganizationId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Abbreviation = abbreviation;
            ParentOrganizationId = parentOrganizationId;
            CreatedAt = DateTime.UtcNow;
            AddDomainEvent(new OrganizationCreatedEvent(this));
        }

        public void SetDefaultEmployeeGroup(Guid employeeGroupId)
        {
            DefaultEmployeeGroupId = employeeGroupId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName) || Name == newName) return;
            Name = newName;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new OrganizationNameChangedEvent(this));
        }

        public void UpdateAbbreviation(string newAbbreviation)
        {
            if (string.IsNullOrWhiteSpace(newAbbreviation) || Abbreviation == newAbbreviation) return;
            Abbreviation = newAbbreviation;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ReassignToParent(Guid? newParentId)
        {
            if (newParentId == Id)
            {
                throw new InvalidOperationException("An organization cannot be its own parent.");
            }
            ParentOrganizationId = newParentId;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new OrganizationParentChangedEvent(this));
        }

        public void PrepareForDeletion(bool deleteSubOrganizations)
        {
            AddDomainEvent(new OrganizationDeletedEvent(this, deleteSubOrganizations));
        }
    }
}
