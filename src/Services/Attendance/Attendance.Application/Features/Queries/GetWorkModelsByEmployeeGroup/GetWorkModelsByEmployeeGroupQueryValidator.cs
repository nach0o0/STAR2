using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelsByEmployeeGroup
{
    public class GetWorkModelsByEmployeeGroupQueryValidator : AbstractValidator<GetWorkModelsByEmployeeGroupQuery>
    {
        public GetWorkModelsByEmployeeGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
