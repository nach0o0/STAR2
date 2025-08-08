using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsForUserInScope
{
    public class GetAssignmentsForUserInScopeQueryValidator : AbstractValidator<GetAssignmentsForUserInScopeQuery>
    {
        public GetAssignmentsForUserInScopeQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
