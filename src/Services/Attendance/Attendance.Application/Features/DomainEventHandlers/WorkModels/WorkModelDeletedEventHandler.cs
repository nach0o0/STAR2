using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Events.WorkModels;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.DomainEventHandlers.WorkModels
{
    public class WorkModelDeletedEventHandler : INotificationHandler<WorkModelDeletedEvent>
    {
        private readonly IEmployeeWorkModelRepository _employeeWorkModelRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public WorkModelDeletedEventHandler(
            IEmployeeWorkModelRepository employeeWorkModelRepository,
            IPublishEndpoint publishEndpoint)
        {
            _employeeWorkModelRepository = employeeWorkModelRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(WorkModelDeletedEvent notification, CancellationToken cancellationToken)
        {
            var workModel = notification.WorkModel;

            // Finde alle Zuweisungen, die dieses Modell verwenden
            var assignmentsToDelete = await _employeeWorkModelRepository.GetByWorkModelIdAsync(workModel.Id, cancellationToken);

            foreach (var assignment in assignmentsToDelete)
            {
                _employeeWorkModelRepository.Delete(assignment);
            }

            // Veröffentliche das Integration Event
            var integrationEvent = new WorkModelDeletedIntegrationEvent
            {
                WorkModelId = workModel.Id,
                EmployeeGroupId = workModel.EmployeeGroupId
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
