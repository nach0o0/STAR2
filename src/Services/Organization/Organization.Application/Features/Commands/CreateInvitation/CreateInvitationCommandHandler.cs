using MediatR;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAuthServiceClient _authServiceClient;
        private readonly IUserContext _userContext;

        public CreateInvitationCommandHandler(
            IInvitationRepository invitationRepository,
            IUserContext userContext,
            IEmployeeRepository employeeRepository,
            IAuthServiceClient authServiceClient)
        {
            _invitationRepository = invitationRepository;
            _userContext = userContext;
            _employeeRepository = employeeRepository;
            _authServiceClient = authServiceClient;
        }

        public async Task<Guid> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            if (!currentUser.EmployeeId.HasValue)
            {
                throw new ForbiddenAccessException("Only employees can create invitations.");
            }

            // 1. Rufe den AuthService auf, um die UserId für die E-Mail zu bekommen.
            var user = await _authServiceClient.GetUserByEmailAsync(command.InviteeEmail, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException("User", command.InviteeEmail);
            }

            // 2. Finde den zugehörigen Employee mit der UserId.
            var inviteeEmployee = await _employeeRepository.GetByUserIdAsync(user.Value.UserId, cancellationToken);
            if (inviteeEmployee is null)
            {
                throw new NotFoundException("Employee profile for user", user.Value.UserId);
            }

            // 3. Erstelle die Einladung mit der gefundenen EmployeeId.
            var invitation = new Invitation(
                currentUser.EmployeeId.Value,
                inviteeEmployee.Id,
                command.TargetEntityType,
                command.TargetEntityId,
                expiresInDays: 7);

            await _invitationRepository.AddAsync(invitation, cancellationToken);

            return invitation.Id;
        }
    }
}
