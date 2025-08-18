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
    public class EmployeeGroupDeletedIntegrationEventConsumer : IConsumer<EmployeeGroupDeletedIntegrationEvent>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly IWorkModelRepository _workModelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeGroupDeletedIntegrationEventConsumer(
            IAttendanceTypeRepository attendanceTypeRepository,
            IPublicHolidayRepository publicHolidayRepository,
            IWorkModelRepository workModelRepository,
            IUnitOfWork unitOfWork)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
            _publicHolidayRepository = publicHolidayRepository;
            _workModelRepository = workModelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<EmployeeGroupDeletedIntegrationEvent> context)
        {
            var groupId = context.Message.EmployeeGroupId;

            // 1. Alle AttendanceTypes für diese Gruppe löschen
            var attendanceTypes = await _attendanceTypeRepository.GetByEmployeeGroupIdAsync(groupId, context.CancellationToken);
            foreach (var type in attendanceTypes)
            {
                _attendanceTypeRepository.Delete(type);
            }

            // 2. Alle PublicHolidays für diese Gruppe löschen
            var publicHolidays = await _publicHolidayRepository.GetByEmployeeGroupIdAsync(groupId, context.CancellationToken);
            foreach (var holiday in publicHolidays)
            {
                _publicHolidayRepository.Delete(holiday);
            }

            // 3. Alle WorkModels für diese Gruppe löschen
            var workModels = await _workModelRepository.GetByEmployeeGroupIdAsync(groupId, context.CancellationToken);
            foreach (var model in workModels)
            {
                _workModelRepository.Delete(model);
            }

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }
}
