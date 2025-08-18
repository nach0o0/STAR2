using Attendance.Application.Interfaces.Persistence;
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
    public class AttendanceEntryCreatedEventHandler : INotificationHandler<AttendanceEntryCreatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public AttendanceEntryCreatedEventHandler(IPublishEndpoint publishEndpoint, IAttendanceTypeRepository attendanceTypeRepository)
        {
            _publishEndpoint = publishEndpoint;
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task Handle(AttendanceEntryCreatedEvent notification, CancellationToken cancellationToken)
        {
            var entry = notification.Entry;

            // Wir müssen den AttendanceType laden, um das `IsPresent`-Flag zu bekommen.
            var attendanceType = await _attendanceTypeRepository.GetByIdAsync(entry.AttendanceTypeId, cancellationToken);
            if (attendanceType is null) return; // Sollte nicht passieren

            var integrationEvent = new AttendanceEntryCreatedOrUpdatedIntegrationEvent
            {
                AttendanceEntryId = entry.Id,
                EmployeeId = entry.EmployeeId,
                Date = entry.Date,
                AttendanceTypeId = entry.AttendanceTypeId,
                IsPresent = attendanceType.IsPresent, // Wichtige Information für andere Services
                Note = entry.Note
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
