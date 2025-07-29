using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeeInfoByUserId
{
    public class GetEmployeeInfoByUserIdQueryValidator : AbstractValidator<GetEmployeeInfoByUserIdQuery>
    {
        public GetEmployeeInfoByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
