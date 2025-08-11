using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.FindUserByEmail
{
    public class FindUserByEmailCommandValidator : AbstractValidator<FindUserByEmailCommand>
    {
        public FindUserByEmailCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
