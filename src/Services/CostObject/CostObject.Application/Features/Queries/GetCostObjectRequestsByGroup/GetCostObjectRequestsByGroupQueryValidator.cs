using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectRequestsByGroup
{
    public class GetCostObjectRequestsByGroupQueryValidator : AbstractValidator<GetCostObjectRequestsByGroupQuery>
    {
        public GetCostObjectRequestsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
