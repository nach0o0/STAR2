using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AddPermissionToRole
{
    public class AddPermissionToRoleCommandHandler : IRequestHandler<AddPermissionToRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public AddPermissionToRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task Handle(AddPermissionToRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            role.AddPermission(command.PermissionId);
        }
    }
}
