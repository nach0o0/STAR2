using MassTransit;
using MediatR;
using Permission.Application.Features.Commands.AssignRoleToUser;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Constants;
using Shared.Domain.Authorization;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.IntegrationEventConsumers
{
    public class EmployeeEmployeeGroupAssignmentChangedConsumer : IConsumer<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISender _sender;

        public EmployeeEmployeeGroupAssignmentChangedConsumer(
            IUserPermissionAssignmentRepository assignmentRepository,
            IRoleRepository roleRepository,
            ISender sender)
        {
            _assignmentRepository = assignmentRepository;
            _roleRepository = roleRepository;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent> context)
        {
            var message = context.Message;

            var employeeRole = await _roleRepository.GetByNameAsync(DefaultRoleDefinitions.EmployeeGroupMemberRole.Name, context.CancellationToken);
            if (employeeRole is null) return;

            // 1. Finde ALLE bestehenden Zuweisungen (Rollen und direkte Permissions) für diesen Benutzer.
            var existingAssignments = await _assignmentRepository.GetAssignmentsForUserAsync(message.UserId, context.CancellationToken);

            // 2. Erstelle ein Set der Scopes, in denen der Benutzer bereits Zuweisungen hat.
            var scopesWithExistingAssignments = existingAssignments
                .Select(a => a.Scope)
                .ToHashSet();

            // 3. Gehe durch alle Gruppen, denen der Benutzer jetzt angehört.
            foreach (var groupId in message.EmployeeGroupIds)
            {
                var newScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";

                // 4. Wenn der Benutzer in diesem neuen Scope noch KEINE Zuweisung hat,
                //    weise ihm die Standard-Rolle zu.
                if (!scopesWithExistingAssignments.Contains(newScope))
                {
                    var command = new AssignRoleToUserCommand(message.UserId, employeeRole.Id, newScope);
                    await _sender.Send(command, context.CancellationToken);
                }
            }
        }
    }
}
