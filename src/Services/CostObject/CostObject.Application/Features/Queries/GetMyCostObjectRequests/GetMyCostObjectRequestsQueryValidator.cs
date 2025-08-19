using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetMyCostObjectRequests
{
    public class GetMyCostObjectRequestsQueryValidator : AbstractValidator<GetMyCostObjectRequestsQuery>
    {
        public GetMyCostObjectRequestsQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
