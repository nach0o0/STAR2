using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByScope
{
    public record GetPermissionsByScopeQueryResult(string Id, string Description);
}
