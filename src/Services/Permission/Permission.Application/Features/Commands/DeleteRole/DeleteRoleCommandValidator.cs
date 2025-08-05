using FluentValidation;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.DeleteRole
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator(IRoleRepository roleRepository)
        {
            RuleFor(x => x.RoleId).NotEmpty();

            RuleFor(x => x.RoleId)
                .MustAsync(async (roleId, cancellation) =>
                {
                    var role = await roleRepository.GetByIdAsync(roleId, cancellation);
                    // Nur benutzerdefinierte Rollen (die einen Scope haben) dürfen gelöscht werden.
                    return role is not null && role.IsCustom;
                })
                .WithMessage("Global roles cannot be deleted.");
        }
    }
}
