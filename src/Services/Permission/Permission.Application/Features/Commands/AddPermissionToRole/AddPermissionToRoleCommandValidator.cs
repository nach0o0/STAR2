using FluentValidation;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AddPermissionToRole
{
    public class AddPermissionToRoleCommandValidator : AbstractValidator<AddPermissionToRoleCommand>
    {
        public AddPermissionToRoleCommandValidator(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.PermissionId).NotEmpty();

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var role = await roleRepository.GetByIdAsync(command.RoleId, cancellation);
                    var permission = await permissionRepository.GetByIdAsync(command.PermissionId, cancellation);
                    if (role is null || permission is null) return true; // Wird im Handler abgefangen.

                    // Wenn die Rolle global ist, kann jede Permission hinzugefügt werden.
                    if (role.Scope is null) return true;

                    // Wenn die Rolle gescoped ist, muss der Scope der Permission passen.
                    var roleScopeType = role.Scope.Split(':')[0];
                    return permission.PermittedScopeTypes.Any(link => link.ScopeType == roleScopeType);
                })
                .WithMessage("This permission cannot be added to a role with this scope.");
        }
    }
}
