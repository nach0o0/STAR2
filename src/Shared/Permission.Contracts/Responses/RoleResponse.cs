using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Responses
{
    public record RoleResponse(
        Guid Id,
        string Name,
        string Description,
        string? Scope,
        List<string> Permissions
    );
}
