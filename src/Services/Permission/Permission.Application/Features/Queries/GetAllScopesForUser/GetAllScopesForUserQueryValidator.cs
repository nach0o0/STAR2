using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAllScopesForUser
{
    public class GetAllScopesForUserQueryValidator : AbstractValidator<GetAllScopesForUserQuery>
    {
        public GetAllScopesForUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
