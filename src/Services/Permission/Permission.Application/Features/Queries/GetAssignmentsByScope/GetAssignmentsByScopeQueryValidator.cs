using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsByScope
{
    public class GetAssignmentsByScopeQueryValidator : AbstractValidator<GetAssignmentsByScopeQuery>
    {
        public GetAssignmentsByScopeQueryValidator()
        {
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
