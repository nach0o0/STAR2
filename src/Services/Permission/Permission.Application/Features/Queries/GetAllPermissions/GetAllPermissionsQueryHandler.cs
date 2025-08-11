using MediatR;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAllPermissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<GetAllPermissionsQueryResult>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<GetAllPermissionsQueryResult>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepository.GetAllAsync(cancellationToken);

            return permissions
                .Select(p => new GetAllPermissionsQueryResult(p.Id, p.Description))
                .ToList();
        }
    }
}
