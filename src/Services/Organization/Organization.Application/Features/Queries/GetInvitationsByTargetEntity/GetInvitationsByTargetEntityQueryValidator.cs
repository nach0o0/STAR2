using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetInvitationsByTargetEntity
{
    public class GetInvitationsByTargetEntityQueryValidator : AbstractValidator<GetInvitationsByTargetEntityQuery>
    {
        public GetInvitationsByTargetEntityQueryValidator()
        {
            RuleFor(x => x.TargetType).IsInEnum();
            RuleFor(x => x.TargetId).NotEmpty();
        }
    }
}
