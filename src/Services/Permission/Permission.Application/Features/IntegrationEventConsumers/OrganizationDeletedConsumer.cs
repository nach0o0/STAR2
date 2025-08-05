using MassTransit;
using Permission.Application.Interfaces.Persistence;
using Shared.Application.Interfaces.Persistence;
using Shared.Domain.Authorization;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.IntegrationEventConsumers
{
    public class OrganizationDeletedConsumer : IConsumer<OrganizationDeletedIntegrationEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrganizationDeletedConsumer(IUserPermissionAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrganizationDeletedIntegrationEvent> context)
        {
            var organizationId = context.Message.OrganizationId;
            var scopeToDelete = $"{PermittedScopeTypes.Organization}:{organizationId}";

            // Finde und lösche alle Zuweisungen für diesen Scope.
            var assignments = await _assignmentRepository.GetAssignmentsByScopeAsync(scopeToDelete, context.CancellationToken);
            if (!assignments.Any()) return;

            foreach (var assignment in assignments)
            {
                _assignmentRepository.Delete(assignment);
            }

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
