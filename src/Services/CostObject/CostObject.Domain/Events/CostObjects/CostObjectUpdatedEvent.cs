using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Events.CostObjects
{
    public record CostObjectUpdatedEvent(Domain.Entities.CostObject CostObject) : INotification;
}
