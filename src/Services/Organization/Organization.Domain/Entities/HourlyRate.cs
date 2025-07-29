using Organization.Domain.Events.HourlyRates;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Entities
{
    public class HourlyRate : BaseEntity<Guid>
    {
        public decimal Rate { get; private set; }
        public string Name { get; private set; }
        public Guid OrganizationId { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }
        public string? Description { get; private set; }

        private HourlyRate() { }

        public HourlyRate(string name, decimal rate, DateTime validFrom, Guid organizationId, DateTime? validTo = null, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Rate = rate;
            ValidFrom = validFrom;
            OrganizationId = organizationId;
            ValidTo = validTo;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string? name,
            decimal? rate,
            DateTime? validFrom,
            DateTime? validTo,
            string? description)
        {
            bool hasChanges = false;
            if (name is not null && !string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                hasChanges = true;
            }
            if (rate.HasValue && Rate != rate.Value)
            {
                Rate = rate.Value;
                hasChanges = true;
            }
            if (validFrom.HasValue && ValidFrom != validFrom.Value)
            {
                ValidFrom = validFrom.Value;
                hasChanges = true;
            }
            if (validTo.HasValue && ValidTo != validTo.Value)
            {
                ValidTo = validTo;
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
            AddDomainEvent(new HourlyRateDeletedEvent(this));
        }
    }
}
