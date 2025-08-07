using Permission.Domain.Events.Roles;
using Permission.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Entities
{
    public class Role : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string? Scope { get; private set; }
        public bool IsCustom { get; private set; }
        public Guid? BaseRoleId { get; private set; }

        private readonly List<RoleToPermissionLink> _permissions = new();
        public IReadOnlyCollection<RoleToPermissionLink> Permissions => _permissions.AsReadOnly();

        private Role() { }

        internal Role(Guid id, string name, string description, string? scope = null)
        {
            Id = id;
            Name = name;
            Description = description;
            IsCustom = false;
            Scope = scope;
            BaseRoleId = null;
            CreatedAt = DateTime.UtcNow;
        }

        public Role(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsCustom = false;
            Scope = null;
            BaseRoleId = null;
            CreatedAt = DateTime.UtcNow;
        }

        public Role(string name, string description, string scope, Guid? baseRoleId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Scope = scope;
            IsCustom = true;
            BaseRoleId = baseRoleId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string? name, string? description)
        {
            bool hasChanges = false;
            if (name is not null && !string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                hasChanges = true;
            }
            if (description is not null && Description != description)
            {
                Description = description;
                hasChanges = true;
            }
            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new RoleDeletedEvent(this));
        }

        public void AddPermission(string permissionId)
        {
            if (_permissions.Any(p => p.PermissionId == permissionId))
            {
                return;
            }
            _permissions.Add(new RoleToPermissionLink(Id, permissionId));
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new PermissionAddedToRoleEvent(Id, permissionId));
        }

        public void RemovePermission(string permissionId)
        {
            var permissionLink = _permissions.FirstOrDefault(p => p.PermissionId == permissionId);
            if (permissionLink is null)
            {
                return;
            }

            _permissions.Remove(permissionLink);
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new PermissionRemovedFromRoleEvent(Id, permissionId));
        }

        public void ClearBaseRole()
        {
            BaseRoleId = null;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
