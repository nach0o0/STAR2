using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Commands.RevokeSession
{
    public class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
    {
        public RevokeSessionCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
