using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetRolesByScope
{
    public class GetRolesByScopeQueryValidator : AbstractValidator<GetRolesByScopeQuery>
    {
        public GetRolesByScopeQueryValidator()
        {
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
