using Attendance.Domain.Events.PublicHolidays;
using MassTransit;
using MediatR;
using Shared.Messages.Events.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.DomainEventHandlers.PublicHolidays
{
    public class PublicHolidayCreatedEventHandler : INotificationHandler<PublicHolidayCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublicHolidayCreatedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(PublicHolidayCreatedEvent notification, CancellationToken cancellationToken)
        {
            var holiday = notification.PublicHoliday;

            var integrationEvent = new PublicHolidayCreatedIntegrationEvent
            {
                PublicHolidayId = holiday.Id,
                Date = holiday.Date,
                Name = holiday.Name,
                EmployeeGroupId = holiday.EmployeeGroupId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
