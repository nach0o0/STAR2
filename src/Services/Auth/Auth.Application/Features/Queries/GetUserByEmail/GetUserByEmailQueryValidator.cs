using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryValidator : AbstractValidator<GetUserByEmailQuery>
    {
        public GetUserByEmailQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
