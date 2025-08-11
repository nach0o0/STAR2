using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Permission.Application.Features.Queries.GetRolesByScope
{
    public class GetRolesByScopeQueryHandler : IRequestHandler<GetRolesByScopeQuery, List<GetRolesByScopeQueryResult>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRolesByScopeQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<GetRolesByScopeQueryResult>> Handle(GetRolesByScopeQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetByScopeAsync(request.Scope, cancellationToken);

            return roles
                .Select(r => new GetRolesByScopeQueryResult(
                    r.Id,
                    r.Name,
                    r.Description,
                    r.Scope,
                    r.Permissions.Select(p => p.PermissionId).ToList()
                ))
                .ToList();
        }
    }
}
