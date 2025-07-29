using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateInvitation
{
    public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Guid>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContext _userContext;

        public CreateInvitationCommandHandler(IInvitationRepository invitationRepository, IUserContext userContext)
        {
            _invitationRepository = invitationRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // Annahme: Einladungen sind 7 Tage gültig.
            var invitation = new Invitation(
                currentUser.EmployeeId,
                command.InviteeEmployeeId,
                command.TargetEntityType,
                command.TargetEntityId,
                expiresInDays: 7);

            await _invitationRepository.AddAsync(invitation, cancellationToken);

            return invitation.Id;
        }
    }
}
