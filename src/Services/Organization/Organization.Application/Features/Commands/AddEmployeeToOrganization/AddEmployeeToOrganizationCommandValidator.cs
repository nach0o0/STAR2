using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.AddEmployeeToOrganization
{
    public class AddEmployeeToOrganizationCommandValidator : AbstractValidator<AddEmployeeToOrganizationCommand>
    {
        public AddEmployeeToOrganizationCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.OrganizationId).NotEmpty();
        }
    }
}
