using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AssignDirectPermissionToUser
{
    public class AssignDirectPermissionToUserCommandHandler : IRequestHandler<AssignDirectPermissionToUserCommand, Guid>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;

        public AssignDirectPermissionToUserCommandHandler(IUserPermissionAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Guid> Handle(AssignDirectPermissionToUserCommand command, CancellationToken cancellationToken)
        {
            var assignment = UserPermissionAssignment.CreateForPermission(
                command.UserId,
                command.Scope,
                command.PermissionId);

            await _assignmentRepository.AddAsync(assignment, cancellationToken);

            return assignment.Id;
        }
    }
}
