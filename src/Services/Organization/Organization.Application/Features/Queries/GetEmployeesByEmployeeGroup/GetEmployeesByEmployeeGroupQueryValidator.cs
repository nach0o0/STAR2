using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByEmployeeGroup
{
    public class GetEmployeesByEmployeeGroupQueryValidator : AbstractValidator<GetEmployeesByEmployeeGroupQuery>
    {
        public GetEmployeesByEmployeeGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
