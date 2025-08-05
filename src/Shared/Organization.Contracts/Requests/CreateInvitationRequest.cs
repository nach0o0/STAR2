using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Requests
{
    public record CreateInvitationRequest(
        string InviteeEmail,
        InvitationTargetEntityType TargetEntityType,
        Guid TargetEntityId
    );
}
