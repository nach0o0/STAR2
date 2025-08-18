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
    public class AttendanceEntryUpdatedEventHandler : INotificationHandler<AttendanceEntryUpdatedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public AttendanceEntryUpdatedEventHandler(IPublishEndpoint publishEndpoint, IAttendanceTypeRepository attendanceTypeRepository)
        {
            _publishEndpoint = publishEndpoint;
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task Handle(AttendanceEntryUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var entry = notification.Entry;

            var attendanceType = await _attendanceTypeRepository.GetByIdAsync(entry.AttendanceTypeId, cancellationToken);
            if (attendanceType is null) return;

            var integrationEvent = new AttendanceEntryCreatedOrUpdatedIntegrationEvent
            {
                AttendanceEntryId = entry.Id,
                EmployeeId = entry.EmployeeId,
                Date = entry.Date,
                AttendanceTypeId = entry.AttendanceTypeId,
                IsPresent = attendanceType.IsPresent,
                Note = entry.Note
            };

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
