using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UnassignWorkModelFromEmployee
{
    public class UnassignWorkModelFromEmployeeCommandValidator : AbstractValidator<UnassignWorkModelFromEmployeeCommand>
    {
        public UnassignWorkModelFromEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeWorkModelId).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
        }
    }
}
