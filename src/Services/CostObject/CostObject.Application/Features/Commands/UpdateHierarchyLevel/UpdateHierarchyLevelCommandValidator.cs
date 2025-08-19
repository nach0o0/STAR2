using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyLevel
{
    public class UpdateHierarchyLevelCommandValidator : AbstractValidator<UpdateHierarchyLevelCommand>
    {
        public UpdateHierarchyLevelCommandValidator()
        {
            RuleFor(x => x.HierarchyLevelId).NotEmpty();

            // Stellt sicher, dass mindestens ein Feld zum Aktualisieren angegeben wird.
            RuleFor(x => x)
                .Must(x => x.Name is not null || x.Depth.HasValue)
                .WithMessage("At least one field (Name or Depth) must be provided for an update.");

            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            });

            When(x => x.Depth.HasValue, () =>
            {
                RuleFor(x => x.Depth).GreaterThanOrEqualTo(0);
            });
        }
    }
}
