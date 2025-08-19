using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectById
{
    public class GetCostObjectByIdQueryValidator : AbstractValidator<GetCostObjectByIdQuery>
    {
        public GetCostObjectByIdQueryValidator()
        {
            RuleFor(x => x.CostObjectId).NotEmpty();
        }
    }
}
