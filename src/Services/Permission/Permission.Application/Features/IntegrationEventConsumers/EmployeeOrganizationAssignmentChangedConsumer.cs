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
    public class EmployeeOrganizationAssignmentChangedConsumer : IConsumer<EmployeeOrganizationAssignmentChangedIntegrationEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISender _sender;

        public EmployeeOrganizationAssignmentChangedConsumer(
            IUserPermissionAssignmentRepository assignmentRepository,
            IRoleRepository roleRepository,
            ISender sender)
        {
            _assignmentRepository = assignmentRepository;
            _roleRepository = roleRepository;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<EmployeeOrganizationAssignmentChangedIntegrationEvent> context)
        {
            var message = context.Message;

            // Reagiere nur, wenn der Mitarbeiter einer Organisation HINZUGEFÜGT wurde.
            if (!message.OrganizationId.HasValue)
            {
                return;
            }

            var employeeRole = await _roleRepository.GetByNameAsync(DefaultRoleDefinitions.OrganizationMemberRole.Name, context.CancellationToken);
            if (employeeRole is null) return;

            var newScope = $"{PermittedScopeTypes.Organization}:{message.OrganizationId.Value}";

            // Prüfe, ob der Benutzer in diesem Scope bereits eine Zuweisung hat.
            var existingAssignments = await _assignmentRepository.GetAssignmentsForUserAsync(message.UserId, new[] { newScope }, context.CancellationToken);

            // Weise die Standard-Rolle nur zu, wenn noch keine andere Zuweisung existiert.
            if (!existingAssignments.Any())
            {
                var command = new AssignRoleToUserCommand(message.UserId, employeeRole.Id, newScope);
                await _sender.Send(command, context.CancellationToken);
            }
        }
    }
}
