using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Events.Organizations
{
    public record OrganizationParentChangedEvent(Entities.Organization Organization) : INotification;
}
