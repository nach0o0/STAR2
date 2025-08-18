using Organization.Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationById
{
    public record GetInvitationByIdQueryResult(
        Guid InvitationId,
        Guid InviterEmployeeId,
        Guid InviteeEmployeeId,
        InvitationTargetEntityType TargetType,
        Guid TargetId,
        InvitationStatus Status,
        DateTime ExpiresAt,
        DateTime CreatedAt
    );
}
