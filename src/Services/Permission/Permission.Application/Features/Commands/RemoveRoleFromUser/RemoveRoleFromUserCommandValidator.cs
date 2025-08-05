using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
    {
        public RemoveRoleFromUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
