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
    public class AttendanceTypeUpdatedEventHandler : INotificationHandler<AttendanceTypeUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public AttendanceTypeUpdatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(AttendanceTypeUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var type = notification.AttendanceType;

            var integrationEvent = new AttendanceTypeUpdatedIntegrationEvent
            {
                AttendanceTypeId = type.Id,
                EmployeeGroupId = type.EmployeeGroupId,
                Name = type.Name,
                Abbreviation = type.Abbreviation,
                IsPresent = type.IsPresent,
                Color = type.Color
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
