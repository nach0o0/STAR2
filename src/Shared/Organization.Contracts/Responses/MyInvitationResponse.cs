using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record MyInvitationResponse(
        Guid InvitationId,
        Guid InviterEmployeeId,
        InvitationTargetEntityType TargetType,
        Guid TargetId,
        DateTime ExpiresAt
    );
}
