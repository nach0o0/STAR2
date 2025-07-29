using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromOrganization
{
    public class RemoveEmployeeFromOrganizationCommandValidator : AbstractValidator<RemoveEmployeeFromOrganizationCommand>
    {
        public RemoveEmployeeFromOrganizationCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}
