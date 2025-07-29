using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateInvitation
{
    public class CreateInvitationCommandValidator : AbstractValidator<CreateInvitationCommand>
    {
        public CreateInvitationCommandValidator()
        {
            RuleFor(x => x.InviteeEmployeeId).NotEmpty();
            RuleFor(x => x.TargetEntityType).IsInEnum();
            RuleFor(x => x.TargetEntityId).NotEmpty();
        }
    }
}
