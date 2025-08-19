using CostObject.Domain.Events.Labels;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Entities
{
    public class Label : BaseEntity<Guid>
    {
        public string Name { get; private set; }
        public Guid EmployeeGroupId { get; private set; }

        private Label() { }

        public Label(string name, Guid employeeGroupId)
        {
            Id = Guid.NewGuid();
            Name = name;
            EmployeeGroupId = employeeGroupId;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new LabelCreatedEvent(this));
        }

        public void Update(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName) || Name == newName)
            {
                return;
            }

            Name = newName;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new LabelUpdatedEvent(this));
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new LabelDeletedEvent(this));
        }
    }
}
