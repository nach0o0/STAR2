using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMyPendingInvitations
{
    public record GetMyPendingInvitationsQueryResult(
        Guid InvitationId,
        Guid InviterEmployeeId,
        InvitationTargetEntityType TargetType,
        Guid TargetId,
        DateTime ExpiresAt
    );
}
