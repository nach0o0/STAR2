using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.CostObjectService
{
    public record CostObjectEntriesReassignedIntegrationEvent
    {
        public Guid SourceCostObjectId { get; init; }
        public Guid DestinationCostObjectId { get; init; }
    }
}
