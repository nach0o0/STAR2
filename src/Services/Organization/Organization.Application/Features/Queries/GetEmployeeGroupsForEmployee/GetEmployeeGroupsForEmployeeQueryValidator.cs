using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeGroupsForEmployee
{
    public class GetEmployeeGroupsForEmployeeQueryValidator : AbstractValidator<GetEmployeeGroupsForEmployeeQuery>
    {
        public GetEmployeeGroupsForEmployeeQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}
