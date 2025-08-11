using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUsersByIds
{
    public class GetUsersByIdsQueryValidator : AbstractValidator<GetUsersByIdsQuery>
    {
        public GetUsersByIdsQueryValidator()
        {
            RuleFor(x => x.UserIds).NotEmpty();
        }
    }
}
