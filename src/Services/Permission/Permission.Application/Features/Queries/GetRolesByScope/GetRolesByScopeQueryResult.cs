using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetRolesByScope
{
    public record GetRolesByScopeQueryResult(
        Guid Id,
        string Name,
        string Description,
        List<string> Permissions
    );
}
