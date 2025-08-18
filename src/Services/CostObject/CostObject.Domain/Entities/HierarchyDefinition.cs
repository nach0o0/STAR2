using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class HierarchyDefinition : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }
        public Guid? RequiredBookingLevelId { get; private set; }

        private HierarchyDefinition() { }

        public HierarchyDefinition(string name, Guid employeeGroupId, Guid? requiredBookingLevelId = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            EmployeeGroupId = employeeGroupId;
            RequiredBookingLevelId = requiredBookingLevelId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
