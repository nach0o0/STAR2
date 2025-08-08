using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByScope
{
    public record GetPermissionsByScopeQuery(string Scope) : IRequest<List<GetPermissionsByScopeQueryResult>>;
}
