using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Events.EmployeeGroups
{
    public record EmployeeGroupTransferredEvent(
        Guid EmployeeGroupId,
        Guid SourceOrganizationId,
        Guid DestinationOrganizationId) : INotification;
}
