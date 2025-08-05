using MediatR;
using Permission.Application.Interfaces.Persistence;
using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Guid> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Role role;
            if (string.IsNullOrEmpty(command.Scope))
            {
                // Globale Rolle erstellen
                role = new Role(command.Name, command.Description);
            }
            else
            {
                // Benutzerdefinierte, gescopte Rolle erstellen
                role = new Role(command.Name, command.Description, command.Scope);
            }

            await _roleRepository.AddAsync(role, cancellationToken);
            return role.Id;
        }
    }
}
