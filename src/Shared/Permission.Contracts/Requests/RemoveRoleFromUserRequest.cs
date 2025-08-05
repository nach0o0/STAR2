using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Requests
{
    public record RemoveRoleFromUserRequest(
        Guid UserId,
        Guid RoleId,
        string Scope
    );
}
