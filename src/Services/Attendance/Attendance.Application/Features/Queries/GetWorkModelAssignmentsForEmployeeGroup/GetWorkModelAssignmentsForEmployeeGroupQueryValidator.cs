using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployeeGroup
{
    public class GetWorkModelAssignmentsForEmployeeGroupQueryValidator : AbstractValidator<GetWorkModelAssignmentsForEmployeeGroupQuery>
    {
        public GetWorkModelAssignmentsForEmployeeGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
