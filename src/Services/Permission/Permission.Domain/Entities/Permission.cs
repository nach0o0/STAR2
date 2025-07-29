using Shared.Domain.Abstractions;
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

        private Permission() { }

        public Permission(string id, string description)
        {
            Id = id;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
