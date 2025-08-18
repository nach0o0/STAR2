using Attendance.Domain.Events.AttendanceTypes;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.DomainEventHandlers.AttendanceTypes
{
    public class AttendanceTypeDeletedEventHandler : INotificationHandler<AttendanceTypeDeletedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public AttendanceTypeDeletedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(AttendanceTypeDeletedEvent notification, CancellationToken cancellationToken)
        {
            var type = notification.AttendanceType;
            var integrationEvent = new AttendanceTypeDeletedIntegrationEvent
            {
                AttendanceTypeId = type.Id,
                EmployeeGroupId = type.EmployeeGroupId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
