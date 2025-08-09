using FluentValidation;
using Microsoft.Extensions.Logging;
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
        public DeleteRoleCommandValidator(IRoleRepository roleRepository, ILogger<DeleteRoleCommandValidator> logger)
        {
            RuleFor(x => x.RoleId).NotEmpty();

            RuleFor(x => x.RoleId)
                .MustAsync(async (roleId, cancellation) =>
                {
                    logger.LogInformation("[VALIDATOR] Checking if role {RoleId} can be deleted.", roleId);
                    var role = await roleRepository.GetByIdAsync(roleId, cancellation);
                    // Nur benutzerdefinierte Rollen (die einen Scope haben) dürfen gelöscht werden.
                    var canBeDeleted = role is not null && role.IsCustom;
                    logger.LogInformation("[VALIDATOR] Role {RoleId} IsCustom = {IsCustom}. Deletion allowed: {CanBeDeleted}", roleId, role?.IsCustom, canBeDeleted);
                    return canBeDeleted;
                })
                .WithMessage("Default system roles cannot be deleted.");
        }
    }
}
