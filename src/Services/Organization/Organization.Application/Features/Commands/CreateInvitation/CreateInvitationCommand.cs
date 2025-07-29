using MediatR;
using Organization.Domain.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateInvitation
{
    public record CreateInvitationCommand(
        Guid InviteeEmployeeId,
        InvitationTargetEntityType TargetEntityType,
        Guid TargetEntityId
    ) : IRequest<Guid>;
}
