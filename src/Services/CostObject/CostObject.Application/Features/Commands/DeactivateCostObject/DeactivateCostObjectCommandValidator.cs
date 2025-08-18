using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeactivateCostObject
{
    public class DeactivateCostObjectCommandValidator : AbstractValidator<DeactivateCostObjectCommand>
    {
        public DeactivateCostObjectCommandValidator()
        {
            RuleFor(x => x.CostObjectId).NotEmpty();
            RuleFor(x => x.ValidTo).NotEmpty().WithMessage("A deactivation date (ValidTo) is required.");
        }
    }
}
