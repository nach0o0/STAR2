using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByUserIds
{
    public class GetEmployeesByUserIdsQueryValidator : AbstractValidator<GetEmployeesByUserIdsQuery>
    {
        public GetEmployeesByUserIdsQueryValidator() => RuleFor(x => x.UserIds).NotEmpty();
    }
}
