using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Invitations
{
    public record InvitationDeclinedIntegrationEvent
    {
        public Guid InvitationId { get; init; }
        public Guid InviterEmployeeId { get; init; }
        public Guid InviteeEmployeeId { get; init; }
    }
}
