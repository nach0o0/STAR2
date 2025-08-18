using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.AssignWorkModelToEmployee
{
    public class AssignWorkModelToEmployeeCommandValidator : AbstractValidator<AssignWorkModelToEmployeeCommand>
    {
        public AssignWorkModelToEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.WorkModelId).NotEmpty();
            RuleFor(x => x.ValidFrom).NotEmpty();

            RuleFor(x => x.ValidTo)
                .GreaterThanOrEqualTo(x => x.ValidFrom)
                .When(x => x.ValidTo.HasValue)
                .WithMessage("ValidTo must be on or after ValidFrom.");
        }
    }
}
