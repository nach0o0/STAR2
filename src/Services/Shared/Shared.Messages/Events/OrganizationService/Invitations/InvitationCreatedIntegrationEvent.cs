using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Invitations
{
    public record InvitationCreatedIntegrationEvent
    {
        public Guid InvitationId { get; init; }
        public Guid InviterEmployeeId { get; init; }
        public Guid InviteeEmployeeId { get; init; }
        public Guid TargetEntityId { get; init; }
        public InvitationTargetEntityType TargetEntityType { get; init; }
    }
}
