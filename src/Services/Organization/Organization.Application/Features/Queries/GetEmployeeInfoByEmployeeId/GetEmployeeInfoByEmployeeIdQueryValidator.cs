using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByEmployeeId
{
    public class GetEmployeeInfoByEmployeeIdQueryValidator : AbstractValidator<GetEmployeeInfoByEmployeeIdQuery>
    {
        public GetEmployeeInfoByEmployeeIdQueryValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}
