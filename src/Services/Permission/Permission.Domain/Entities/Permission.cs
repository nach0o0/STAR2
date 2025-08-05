using Permission.Domain.Events.Permissions;
using Permission.Domain.ValueObjects;
using Shared.Domain.Abstractions;
using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Entities
{
    public class Permission : BaseEntity<string>
    {
        public string Description { get; private set; }

        private readonly List<PermissionToScopeTypeLink> _permittedScopeTypes = new();
        public IReadOnlyCollection<PermissionToScopeTypeLink> PermittedScopeTypes => _permittedScopeTypes.AsReadOnly();

        private Permission() { }

        public Permission(string id, string description, List<string> permittedScopeTypes)
        {
            Id = id;
            Description = description;
            CreatedAt = DateTime.UtcNow;

            // Erstellt die Link-Objekte bei der Erzeugung der Entität.
            _permittedScopeTypes = permittedScopeTypes
                .Select(scopeType => new PermissionToScopeTypeLink(id, scopeType))
                .ToList();
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new PermissionDeletedEvent(this));
        }
    }
}
