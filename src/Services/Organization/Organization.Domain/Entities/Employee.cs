using Organization.Domain.Events.Employees;
using Organization.Domain.ValueObjects;
using Shared.Domain.Abstractions;

namespace Organization.Domain.Entities
{
    public class Employee : BaseEntity<Guid>
    {
        public Guid? UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Guid? OrganizationId { get; private set; }

        private readonly List<EmployeeToEmployeeGroupLink> _employeeGroupLinks = new();
        public IReadOnlyCollection<EmployeeToEmployeeGroupLink> EmployeeGroupLinks => _employeeGroupLinks.AsReadOnly();

        private Employee() { }

        public Employee(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            CreatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeCreatedEvent(this));
        }

        public void UpdateDetails(string? firstName, string? lastName)
        {
            bool hasChanges = false;
            if (firstName is not null && !string.IsNullOrWhiteSpace(firstName) && FirstName != firstName)
            {
                FirstName = firstName;
                hasChanges = true;
            }
            if (lastName is not null && !string.IsNullOrWhiteSpace(lastName) && LastName != lastName)
            {
                LastName = lastName;
                hasChanges = true;
            }
            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new EmployeeDetailsUpdatedEvent(this));
            }
        }

        public void AssignToOrganization(Guid organizationId)
        {
            OrganizationId = organizationId;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeAssignedToOrganizationEvent(Id, organizationId));
        }

        public void UnassignFromOrganization()
        {
            if (!OrganizationId.HasValue)
            {
                return;
            }
            var previousOrganizationId = OrganizationId.Value;
            OrganizationId = null;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeUnassignedFromOrganizationEvent(this, previousOrganizationId));
        }

        public void AssignToGroup(Guid employeeGroupId, Guid? hourlyRateId)
        {
            if (_employeeGroupLinks.Any(l => l.EmployeeGroupId == employeeGroupId))
            {
                // Verhindert doppelte Einträge
                return;
            }
            _employeeGroupLinks.Add(new EmployeeToEmployeeGroupLink(this.Id, employeeGroupId, hourlyRateId));
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeAssignedToGroupEvent(Id, employeeGroupId));
        }

        public void UnassignFromGroup(Guid employeeGroupId, Organization groupOrganization)
        {
            if (employeeGroupId == groupOrganization.DefaultEmployeeGroupId &&
                this.OrganizationId == groupOrganization.Id)
            {
                throw new InvalidOperationException("Primary members cannot be removed from their organization's default group.");
            }
            var linkToRemove = _employeeGroupLinks.FirstOrDefault(l => l.EmployeeGroupId == employeeGroupId);
            if (linkToRemove is null)
            {
                return;
            }
            _employeeGroupLinks.Remove(linkToRemove);
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new EmployeeUnassignedFromGroupEvent(Id, employeeGroupId));
        }

        public void AssignUser(Guid userId)
        {
            if (UserId.HasValue)
            {
                throw new InvalidOperationException("Employee is already assigned to a user.");
            }
            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UserAssignedToEmployeeEvent(Id, userId));
        }

        public void UnassignUser()
        {
            if (!UserId.HasValue)
            {
                return;
            }
            var previousUserId = UserId.Value;
            UserId = null;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UserUnassignedFromEmployeeEvent(Id, previousUserId));
        }

        public void UpdateHourlyRateForGroup(Guid employeeGroupId, Guid? newHourlyRateId)
        {
            var link = _employeeGroupLinks.FirstOrDefault(l => l.EmployeeGroupId == employeeGroupId);
            if (link is null)
            {
                throw new InvalidOperationException("Employee is not a member of the specified group.");
            }
            link.SetHourlyRate(newHourlyRateId);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
