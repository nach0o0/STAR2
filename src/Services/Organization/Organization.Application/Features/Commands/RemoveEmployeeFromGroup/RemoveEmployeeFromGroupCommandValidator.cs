using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.RemoveEmployeeFromGroup
{
    public class RemoveEmployeeFromGroupCommandValidator : AbstractValidator<RemoveEmployeeFromGroupCommand>
    {
        public RemoveEmployeeFromGroupCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
