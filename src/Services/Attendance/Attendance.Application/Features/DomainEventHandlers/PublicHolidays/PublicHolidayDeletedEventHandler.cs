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
    public class PublicHolidayDeletedEventHandler : INotificationHandler<PublicHolidayDeletedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublicHolidayDeletedEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Handle(PublicHolidayDeletedEvent notification, CancellationToken cancellationToken)
        {
            var holiday = notification.PublicHoliday;

            var integrationEvent = new PublicHolidayDeletedIntegrationEvent
            {
                PublicHolidayId = holiday.Id,
                EmployeeGroupId = holiday.EmployeeGroupId
            };

            return _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
