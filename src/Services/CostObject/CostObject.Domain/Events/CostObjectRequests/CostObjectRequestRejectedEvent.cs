using CostObject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Domain.Events.CostObjectRequests
{
    public record CostObjectRequestRejectedEvent(
        CostObjectRequest CostObjectRequest,
        Guid? ReassignmentCostObjectId
    ) : INotification;
}
