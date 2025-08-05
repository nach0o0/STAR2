using FluentValidation;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.AssignDirectPermissionToUser
{
    public class AssignDirectPermissionToUserCommandValidator : AbstractValidator<AssignDirectPermissionToUserCommand>
    {
        public AssignDirectPermissionToUserCommandValidator(IPermissionRepository permissionRepository)
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PermissionId).NotEmpty();
            RuleFor(x => x.Scope).NotEmpty();

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var permission = await permissionRepository.GetByIdAsync(command.PermissionId, cancellation);
                    if (permission is null) return true;

                    var commandScopeTypeString = command.Scope.Split(':')[0];

                    return permission.PermittedScopeTypes.Any(link => link.ScopeType == commandScopeTypeString);
                })
                .WithMessage("This permission cannot be assigned to this type of scope.");
        }
    }
}
