using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByRole
{
    public record GetPermissionsByRoleQuery(Guid RoleId)
        : IRequest<List<GetPermissionsByRoleQueryResult>>;
}
