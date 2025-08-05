using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
            if (role is null)
            {
                throw new NotFoundException(nameof(Role), command.RoleId);
            }

            role.Update(command.Name, command.Description);
        }
    }
}
