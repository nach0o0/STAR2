using MediatR;
using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeclineInvitation
{
    public record DeclineInvitationCommand(Guid InvitationId) : ICommand;
}
