using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandValidator : AbstractValidator<DeleteMyAccountCommand>
    {
        public DeleteMyAccountCommandValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
