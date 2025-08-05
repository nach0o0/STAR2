using MassTransit;
using MediatR;
using Organization.Application.Features.Commands.DeleteEmployeeGroup;
using Organization.Application.Features.Commands.DeleteOrganization;
using Organization.Application.Features.Commands.ReassignOrganizationParent;
using Organization.Application.Features.Commands.RemoveEmployeeFromOrganization;
using Organization.Application.Features.Commands.RevokeInvitation;
using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Events.Organizations;
using Shared.Messages.Events.OrganizationService;

namespace Organization.Application.Features.DomainEventHandlers.Organizations
{
    public class OrganizationDeletedEventHandler : INotificationHandler<OrganizationDeletedEvent>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IEmployeeGroupRepository _employeeGroupRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly ISender _sender;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrganizationDeletedEventHandler(
            IOrganizationRepository organizationRepository,
            IEmployeeGroupRepository employeeGroupRepository,
            IEmployeeRepository employeeRepository,
            IInvitationRepository invitationRepository,
            ISender sender,
            IPublishEndpoint publishEndpoint)
        {
            _organizationRepository = organizationRepository;
            _employeeGroupRepository = employeeGroupRepository;
            _employeeRepository = employeeRepository;
            _invitationRepository = invitationRepository;
            _sender = sender;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(OrganizationDeletedEvent notification, CancellationToken cancellationToken)
        {
            var deletedOrganization = notification.Organization;

            // 1. Alle Mitarbeiter von der Organisation entbinden
            var employees = await _employeeRepository.GetByOrganizationIdAsync(deletedOrganization.Id, cancellationToken);
            foreach (var employee in employees)
            {
                await _sender.Send(new RemoveEmployeeFromOrganizationCommand(employee.Id), cancellationToken);
            }

            // 2. Alle offenen Einladungen für diese Organisation löschen
            var invitations = await _invitationRepository.GetForOrganizationAsync(deletedOrganization.Id, cancellationToken);
            foreach (var invitation in invitations)
            {
                await _sender.Send(new RevokeInvitationCommand(invitation.Id), cancellationToken);
            }

            // 3. Alle EmployeeGroups der Organisation löschen
            var groupsToDelete = await _employeeGroupRepository.GetByLeadingOrganizationIdAsync(deletedOrganization.Id, cancellationToken);
            foreach (var group in groupsToDelete)
            {
                await _sender.Send(new DeleteEmployeeGroupCommand(group.Id), cancellationToken);
            }

            // 4. Sub-Organisationen behandeln
            var subOrgs = await _organizationRepository.GetSubOrganizationsAsync(deletedOrganization.Id, cancellationToken);
            if (notification.DeleteSubOrganizations)
            {
                foreach (var subOrg in subOrgs)
                {
                    await _sender.Send(new DeleteOrganizationCommand(subOrg.Id, true), cancellationToken);
                }
            }
            else
            {
                foreach (var subOrg in subOrgs)
                {
                    await _sender.Send(
                        new ReassignOrganizationToParentCommand(subOrg.Id, deletedOrganization.ParentOrganizationId),
                        cancellationToken);
                }
            }

            // 5. Integration Event über MassTransit veröffentlichen
            var integrationEvent = new OrganizationDeletedIntegrationEvent { OrganizationId = deletedOrganization.Id };
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
