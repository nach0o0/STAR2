using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemoveDirectPermissionFromUser
{
    public class RemoveDirectPermissionFromUserCommandValidator : AbstractValidator<RemoveDirectPermissionFromUserCommand>
    {
        public RemoveDirectPermissionFromUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PermissionId).NotEmpty();
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
