using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.CostObjectService
{
    public record CostObjectDeletedIntegrationEvent
    {
        public Guid CostObjectId { get; init; }
    }
}
