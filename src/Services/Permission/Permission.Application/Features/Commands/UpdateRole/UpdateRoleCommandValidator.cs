using FluentValidation;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator(IRoleRepository roleRepository)
        {
            RuleFor(x => x.RoleId).NotEmpty();

            // Prüft, ob der neue Name, falls vorhanden, nicht bereits vergeben ist.
            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x)
                    .MustAsync(async (command, cancellation) =>
                    {
                        var role = await roleRepository.GetByIdAsync(command.RoleId, cancellation);
                        if (role is null) return true; // Wird vom Authorizer/Handler abgefangen.
                        if (role.Name.Equals(command.Name, StringComparison.OrdinalIgnoreCase)) return true;
                        return !await roleRepository.NameExistsInScopeAsync(command.Name!, role.Scope, cancellation);
                    })
                    .WithMessage("A role with this name already exists in this scope.");
            });
        }
    }
}
