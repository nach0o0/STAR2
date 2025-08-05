using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record OrganizationCreatedIntegrationEvent
    {
        public Guid OrganizationId { get; init; }
        public string Name { get; init; }
        public Guid CreatorUserId { get; init; }
    }
}
