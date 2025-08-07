using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.SeedInitialAdmin
{
    public class SeedInitialAdminCommandHandler : IRequestHandler<SeedInitialAdminCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public SeedInitialAdminCommandHandler(IRoleRepository roleRepository, IUserPermissionAssignmentRepository assignmentRepository)
        {
            _roleRepository = roleRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task Handle(SeedInitialAdminCommand command, CancellationToken cancellationToken)
        {
            // Überprüfen, ob die angegebene Rolle existiert
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            // Die Zuweisung erstellen und speichern
            var assignment = UserPermissionAssignment.CreateForRole(command.UserId, command.Scope, role.Id);
            await _assignmentRepository.AddAsync(assignment, cancellationToken);
        }
    }
}
