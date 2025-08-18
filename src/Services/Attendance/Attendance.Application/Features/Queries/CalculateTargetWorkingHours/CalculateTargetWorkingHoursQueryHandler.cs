using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.CalculateTargetWorkingHours
{
    public class CalculateTargetWorkingHoursQueryHandler
        : IRequestHandler<CalculateTargetWorkingHoursQuery, CalculateTargetWorkingHoursQueryResult>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;
        private readonly IWorkModelRepository _workModelRepository;
        private readonly IPublicHolidayRepository _publicHolidayRepository;
        private readonly IAttendanceEntryRepository _attendanceEntryRepository; // Hinzugefügt
        private readonly IOrganizationServiceClient _organizationServiceClient;

        public CalculateTargetWorkingHoursQueryHandler(
            IEmployeeWorkModelRepository assignmentRepository,
            IWorkModelRepository workModelRepository,
            IPublicHolidayRepository publicHolidayRepository,
            IAttendanceEntryRepository attendanceEntryRepository, // Hinzugefügt
            IOrganizationServiceClient organizationServiceClient)
        {
            _assignmentRepository = assignmentRepository;
            _workModelRepository = workModelRepository;
            _publicHolidayRepository = publicHolidayRepository;
            _attendanceEntryRepository = attendanceEntryRepository; // Hinzugefügt
            _organizationServiceClient = organizationServiceClient;
        }

        public async Task<CalculateTargetWorkingHoursQueryResult> Handle(
            CalculateTargetWorkingHoursQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Hole alle relevanten Daten
            var assignments = await _assignmentRepository.GetAssignmentsForEmployeeByDateRangeAsync(request.EmployeeId, request.StartDate, request.EndDate, cancellationToken);
            if (!assignments.Any()) return new CalculateTargetWorkingHoursQueryResult(0);

            var workModelIds = assignments.Select(a => a.WorkModelId).Distinct();
            var workModels = (await _workModelRepository.GetByIdsAsync(workModelIds, cancellationToken)).ToDictionary(wm => wm.Id);

            // Hole die Gruppen-IDs über die UserId, die mit der EmployeeId verknüpft ist.
            var employeeInfo = await _organizationServiceClient.GetEmployeeInfoByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            var publicHolidays = await _publicHolidayRepository.GetByDateRangeAndGroupsAsync(
                employeeInfo?.EmployeeGroupIds ?? new List<Guid>(), request.StartDate, request.EndDate, cancellationToken);
            var holidayDates = publicHolidays.Select(ph => ph.Date).ToHashSet();

            // Lade alle Anwesenheitseinträge für den Zeitraum
            var attendanceEntries = (await _attendanceEntryRepository.GetForEmployeeInGroupsByDateRangeAsync(
                request.EmployeeId,
                employeeInfo?.EmployeeGroupIds ?? new List<Guid>(),
                request.StartDate,
                request.EndDate,
                cancellationToken))
                .ToDictionary(e => e.Date);


            // 2. Iteriere durch jeden Tag und berechne die Soll-Stunden
            decimal totalTargetHours = 0;
            for (var day = request.StartDate.Date; day <= request.EndDate.Date; day = day.AddDays(1))
            {
                if (holidayDates.Contains(day)) continue; // Keine Soll-Stunden an Feiertagen

                var activeAssignment = assignments.FirstOrDefault(a => day >= a.ValidFrom && (a.ValidTo == null || day <= a.ValidTo));
                if (activeAssignment != null && workModels.TryGetValue(activeAssignment.WorkModelId, out var workModel))
                {
                    // Ermittle die planmäßigen Stunden für diesen Wochentag
                    var hoursForDay = GetHoursForDayOfWeek(workModel, day.DayOfWeek);

                    // Prüfe, ob ein Abwesenheitseintrag für diesen Tag existiert
                    if (attendanceEntries.TryGetValue(day, out var entry) && !entry.AttendanceType.IsPresent)
                    {
                        // Tag ist eine Abwesenheit, also 0 Soll-Stunden für diesen Tag
                        continue;
                    }

                    totalTargetHours += hoursForDay;
                }
            }

            return new CalculateTargetWorkingHoursQueryResult(totalTargetHours);
        }

        private decimal GetHoursForDayOfWeek(WorkModel workModel, DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => workModel.MondayHours,
                DayOfWeek.Tuesday => workModel.TuesdayHours,
                DayOfWeek.Wednesday => workModel.WednesdayHours,
                DayOfWeek.Thursday => workModel.ThursdayHours,
                DayOfWeek.Friday => workModel.FridayHours,
                DayOfWeek.Saturday => workModel.SaturdayHours,
                DayOfWeek.Sunday => workModel.SundayHours,
                _ => 0
            };
        }
    }
}
