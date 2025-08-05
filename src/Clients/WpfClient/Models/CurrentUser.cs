using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public record CurrentUser(
        Guid UserId,
        string Email,
        Guid? EmployeeId,
        Guid? OrganizationId,
        IReadOnlyList<Guid> EmployeeGroupIds,
        IReadOnlyDictionary<string, List<string>> PermissionsByScope
    );
}
