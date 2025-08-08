using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetRolesByScope
{
    public record GetRolesByScopeQuery(string Scope) : IRequest<List<GetRolesByScopeQueryResult>>;
}
