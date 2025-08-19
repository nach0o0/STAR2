using CostObject.Domain.Events.HierarchyLevels;
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

            AddDomainEvent(new HierarchyLevelCreatedEvent(this));
        }

        public void Update(string? newName, int? newDepth)
        {
            bool hasChanges = false;
            if (newName is not null && !string.IsNullOrWhiteSpace(newName) && Name != newName)
            {
                Name = newName;
                hasChanges = true;
            }
            if (newDepth.HasValue && Depth != newDepth.Value)
            {
                Depth = newDepth.Value;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdatedAt = DateTime.UtcNow;
                AddDomainEvent(new HierarchyLevelUpdatedEvent(this));
            }
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new HierarchyLevelDeletedEvent(this));
        }
    }
}
