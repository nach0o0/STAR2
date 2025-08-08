using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByRole
{
    public class GetPermissionsByRoleQueryHandler : IRequestHandler<GetPermissionsByRoleQuery, List<GetPermissionsByRoleQueryResult>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionsByRoleQueryHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<List<GetPermissionsByRoleQueryResult>> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
        {
            var permissionIds = await _roleRepository.GetPermissionsForRoleAsync(request.RoleId, cancellationToken);
            var result = new List<GetPermissionsByRoleQueryResult>();

            foreach (var id in permissionIds)
            {
                var permission = await _permissionRepository.GetByIdAsync(id, cancellationToken);
                if (permission != null)
                {
                    result.Add(new GetPermissionsByRoleQueryResult(permission.Id, permission.Description));
                }
            }
            return result;
        }
    }
}
