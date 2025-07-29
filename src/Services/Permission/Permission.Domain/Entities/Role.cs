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

        private Role() { }

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
    }
}
