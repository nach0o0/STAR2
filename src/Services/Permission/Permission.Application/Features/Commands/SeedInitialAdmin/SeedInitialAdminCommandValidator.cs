using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.SeedInitialAdmin
{
    public class SeedInitialAdminCommandValidator : AbstractValidator<SeedInitialAdminCommand>
    {
        public SeedInitialAdminCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RoleId).NotEmpty();
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
