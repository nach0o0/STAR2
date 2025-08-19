using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetChildCostObjects
{
    public class GetChildCostObjectsQueryValidator : AbstractValidator<GetChildCostObjectsQuery>
    {
        public GetChildCostObjectsQueryValidator()
        {
            RuleFor(x => x.ParentCostObjectId).NotEmpty();
        }
    }
}
