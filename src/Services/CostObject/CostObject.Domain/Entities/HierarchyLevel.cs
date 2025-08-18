using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class HierarchyLevel : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public int Depth { get; private set; }
        public Guid HierarchyDefinitionId { get; private set; }

        private HierarchyLevel() { }

        public HierarchyLevel(string name, int depth, Guid hierarchyDefinitionId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Depth = depth;
            HierarchyDefinitionId = hierarchyDefinitionId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
