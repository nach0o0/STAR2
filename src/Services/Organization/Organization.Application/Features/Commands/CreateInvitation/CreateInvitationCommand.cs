using MediatR;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Messaging;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateInvitation
{
    public record CreateInvitationCommand(
        string InviteeEmail,
        InvitationTargetEntityType TargetEntityType,
        Guid TargetEntityId
    ) : ICommand<Guid>;
}
