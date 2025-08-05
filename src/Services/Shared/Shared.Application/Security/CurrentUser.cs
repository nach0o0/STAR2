using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Security
{
    public record CurrentUser(
        Guid UserId,
        Guid? EmployeeId,
        Guid? OrganizationId,
        IReadOnlyList<Guid> EmployeeGroupIds,
        IReadOnlyDictionary<string, List<string>> PermissionsByScope
    );
}
