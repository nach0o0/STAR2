using Organization.Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetMySentInvitations
{
    public record GetMySentInvitationsQueryResult(
        Guid InvitationId,
        Guid InviteeEmployeeId,
        InvitationTargetEntityType TargetType,
        Guid TargetId,
        InvitationStatus Status,
        DateTime ExpiresAt
    );
}
