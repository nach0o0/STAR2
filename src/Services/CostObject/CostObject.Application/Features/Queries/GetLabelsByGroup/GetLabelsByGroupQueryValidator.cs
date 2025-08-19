using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetLabelsByGroup
{
    public class GetLabelsByGroupQueryValidator : AbstractValidator<GetLabelsByGroupQuery>
    {
        public GetLabelsByGroupQueryValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
