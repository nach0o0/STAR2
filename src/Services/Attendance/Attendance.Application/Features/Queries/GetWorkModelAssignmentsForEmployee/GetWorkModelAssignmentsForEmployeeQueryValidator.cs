using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployee
{
    public class GetWorkModelAssignmentsForEmployeeQueryValidator : AbstractValidator<GetWorkModelAssignmentsForEmployeeQuery>
    {
        public GetWorkModelAssignmentsForEmployeeQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}
