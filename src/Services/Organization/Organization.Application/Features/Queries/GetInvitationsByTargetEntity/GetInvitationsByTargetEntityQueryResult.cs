using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationsByTargetEntity
{
    public record GetInvitationsByTargetEntityQueryResult(
        Guid InvitationId,
        Guid InviteeEmployeeId,
        DateTime ExpiresAt
    );
}
