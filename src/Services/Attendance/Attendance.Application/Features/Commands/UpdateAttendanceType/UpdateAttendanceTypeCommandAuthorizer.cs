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

namespace Attendance.Application.Features.Commands.UpdateAttendanceType
{
    public class UpdateAttendanceTypeCommandAuthorizer : ICommandAuthorizer<UpdateAttendanceTypeCommand>
    {
        private readonly IAttendanceTypeRepository _attendanceTypeRepository;

        public UpdateAttendanceTypeCommandAuthorizer(IAttendanceTypeRepository attendanceTypeRepository)
        {
            _attendanceTypeRepository = attendanceTypeRepository;
        }

        public async Task AuthorizeAsync(UpdateAttendanceTypeCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var attendanceType = await _attendanceTypeRepository.GetByIdAsync(command.AttendanceTypeId, cancellationToken);
            if (attendanceType is null)
            {
                throw new NotFoundException(nameof(AttendanceType), command.AttendanceTypeId);
            }

            var requiredPermission = AttendanceTypePermissions.Update;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{attendanceType.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update attendance types for this employee group is required.");
            }
        }
    }
}
