using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.RegisterPermissions
{
    public class RegisterPermissionsCommandValidator : AbstractValidator<RegisterPermissionsCommand>
    {
        public RegisterPermissionsCommandValidator()
        {
            RuleFor(x => x.Permissions).NotEmpty();

            RuleForEach(x => x.Permissions).ChildRules(permission =>
            {
                permission.RuleFor(p => p.Id).NotEmpty();
                permission.RuleFor(p => p.Description).NotEmpty();
                permission.RuleFor(p => p.PermittedScopeTypes).NotEmpty();
            });
        }
    }
}
