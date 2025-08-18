using Attendance.Application.Interfaces.Persistence;
using Attendance.Domain.Authorization;
using Attendance.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeletePublicHoliday
{
    public class DeletePublicHolidayCommandAuthorizer : ICommandAuthorizer<DeletePublicHolidayCommand>
    {
        private readonly IPublicHolidayRepository _publicHolidayRepository;

        public DeletePublicHolidayCommandAuthorizer(IPublicHolidayRepository publicHolidayRepository)
        {
            _publicHolidayRepository = publicHolidayRepository;
        }

        public async Task AuthorizeAsync(DeletePublicHolidayCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var publicHoliday = await _publicHolidayRepository.GetByIdAsync(command.PublicHolidayId, cancellationToken);
            if (publicHoliday is null)
            {
                throw new NotFoundException(nameof(PublicHoliday), command.PublicHolidayId);
            }

            var requiredPermission = PublicHolidayPermissions.Delete;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{publicHoliday.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to delete public holidays for this employee group is required.");
            }
        }
    }
}
