using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemovePermissionFromRole
{
    public class RemovePermissionFromRoleCommandValidator : AbstractValidator<RemovePermissionFromRoleCommand>
    {
        public RemovePermissionFromRoleCommandValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.PermissionId).NotEmpty();
        }
    }
}
