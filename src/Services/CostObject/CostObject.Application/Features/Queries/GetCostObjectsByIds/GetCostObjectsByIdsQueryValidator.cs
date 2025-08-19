using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByIds
{
    public class GetCostObjectsByIdsQueryValidator : AbstractValidator<GetCostObjectsByIdsQuery>
    {
        public GetCostObjectsByIdsQueryValidator()
        {
            RuleFor(x => x.CostObjectIds)
                .NotEmpty().WithMessage("The list of cost object IDs cannot be empty.")
                .Must(ids => ids != null && ids.Any()).WithMessage("You must provide at least one cost object ID.");
        }
    }
}
