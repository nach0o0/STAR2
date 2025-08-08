using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetPermissionsByScope
{
    public class GetPermissionsByScopeQueryValidator : AbstractValidator<GetPermissionsByScopeQuery>
    {
        public GetPermissionsByScopeQueryValidator()
        {
            RuleFor(x => x.Scope).NotEmpty();
        }
    }
}
