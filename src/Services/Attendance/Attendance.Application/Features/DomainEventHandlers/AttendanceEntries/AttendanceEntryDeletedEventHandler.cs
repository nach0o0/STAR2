using Attendance.Domain.Events.AttendanceEntries;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.DomainEventHandlers.AttendanceEntries
{
    public class AttendanceEntryDeletedEventHandler : INotificationHandler<AttendanceEntryDeletedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public AttendanceEntryDeletedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(AttendanceEntryDeletedEvent notification, CancellationToken cancellationToken)
        {
            var entry = notification.Entry;

            var integrationEvent = new AttendanceEntryDeletedIntegrationEvent
            {
                AttendanceEntryId = entry.Id,
                EmployeeId = entry.EmployeeId,
                Date = entry.Date
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
