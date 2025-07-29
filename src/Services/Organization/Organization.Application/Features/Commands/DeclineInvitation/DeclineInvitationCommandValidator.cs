using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeclineInvitation
{
    public class DeclineInvitationCommandValidator : AbstractValidator<DeclineInvitationCommand>
    {
        public DeclineInvitationCommandValidator()
        {
            RuleFor(x => x.InvitationId).NotEmpty();
        }
    }
}
