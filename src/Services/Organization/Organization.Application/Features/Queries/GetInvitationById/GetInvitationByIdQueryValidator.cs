using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationById
{
    public class GetInvitationByIdQueryValidator : AbstractValidator<GetInvitationByIdQuery>
    {
        public GetInvitationByIdQueryValidator()
        {
            RuleFor(x => x.InvitationId).NotEmpty();
        }
    }
}
