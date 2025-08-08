using MediatR;
using Permission.Application.Interfaces.Persistence;
using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByScope
{
    public class GetPermissionsByScopeQueryHandler : IRequestHandler<GetPermissionsByScopeQuery, List<GetPermissionsByScopeQueryResult>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionsByScopeQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<GetPermissionsByScopeQueryResult>> Handle(GetPermissionsByScopeQuery request, CancellationToken cancellationToken)
        {
            var scopeType = request.Scope.Split(':')[0];
            if (string.IsNullOrEmpty(scopeType))
            {
                scopeType = PermittedScopeTypes.Global;
            }

            var permissions = await _permissionRepository.GetByScopeTypeAsync(scopeType, cancellationToken);

            return permissions
                .Select(p => new GetPermissionsByScopeQueryResult(p.Id, p.Description))
                .ToList();
        }
    }
}
