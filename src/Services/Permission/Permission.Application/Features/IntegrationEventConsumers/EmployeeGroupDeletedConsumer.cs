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
    public class EmployeeGroupDeletedConsumer : IConsumer<EmployeeGroupDeletedIntegrationEvent>
    {
        private readonly IUserPermissionAssignmentRepository _assignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeGroupDeletedConsumer(IUserPermissionAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EmployeeGroupDeletedIntegrationEvent> context)
        {
            var employeeGroupId = context.Message.EmployeeGroupId;
            var scopeToDelete = $"{PermittedScopeTypes.EmployeeGroup}:{employeeGroupId}";

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
