using MediatR;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationsByTargetEntity
{
    public record GetInvitationsByTargetEntityQuery(
        InvitationTargetEntityType TargetType,
        Guid TargetId)
        : IRequest<List<GetInvitationsByTargetEntityQueryResult>>;
}
