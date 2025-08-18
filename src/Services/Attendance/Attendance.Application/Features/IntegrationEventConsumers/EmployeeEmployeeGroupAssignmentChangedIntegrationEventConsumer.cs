using Attendance.Application.Interfaces.Persistence;
using MassTransit;
using Shared.Application.Interfaces.Persistence;
using Shared.Messages.Events.OrganizationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.IntegrationEventConsumers
{
    public class EmployeeEmployeeGroupAssignmentChangedIntegrationEventConsumer : IConsumer<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;
        private readonly IWorkModelRepository _workModelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeEmployeeGroupAssignmentChangedIntegrationEventConsumer(
            IEmployeeWorkModelRepository assignmentRepository,
            IWorkModelRepository workModelRepository,
            IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _workModelRepository = workModelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EmployeeEmployeeGroupAssignmentChangedIntegrationEvent> context)
        {
            var employeeId = context.Message.EmployeeId;
            var newGroupIds = new HashSet<Guid>(context.Message.EmployeeGroupIds);

            // 1. Hole alle aktuellen (nicht beendeten) Zuweisungen des Mitarbeiters
            var activeAssignments = await _assignmentRepository.GetActiveAssignmentsForEmployeeAsync(employeeId, context.CancellationToken);

            foreach (var assignment in activeAssignments)
            {
                // 2. Lade das zugehörige Arbeitsmodell, um dessen Gruppenzugehörigkeit zu prüfen
                var workModel = await _workModelRepository.GetByIdAsync(assignment.WorkModelId, context.CancellationToken);

                // 3. Wenn das Arbeitsmodell zu einer Gruppe gehört, in der der Mitarbeiter NICHT MEHR ist...
                if (workModel != null && !newGroupIds.Contains(workModel.EmployeeGroupId))
                {
                    // ...beende die Zuweisung zum heutigen Tag.
                    assignment.EndAssignment(DateTime.UtcNow.Date);
                }
            }

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
