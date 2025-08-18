using Auth.Application.Features.Commands.ChangePassword;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.PrivilegedResetPassword
{
    public class PrivilegedResetPasswordCommandValidator : AbstractValidator<PrivilegedResetPasswordCommand>
    {
        public PrivilegedResetPasswordCommandValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty();
        }
    }
}
