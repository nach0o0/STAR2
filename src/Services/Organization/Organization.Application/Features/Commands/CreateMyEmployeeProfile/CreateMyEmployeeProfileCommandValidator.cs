using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateMyEmployeeProfile
{
    public class CreateMyEmployeeProfileCommandValidator : AbstractValidator<CreateMyEmployeeProfileCommand>
    {
        public CreateMyEmployeeProfileCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
