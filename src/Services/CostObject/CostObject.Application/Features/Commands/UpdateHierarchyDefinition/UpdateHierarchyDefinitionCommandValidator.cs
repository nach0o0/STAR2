using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateHierarchyDefinition
{
    public class UpdateHierarchyDefinitionCommandValidator : AbstractValidator<UpdateHierarchyDefinitionCommand>
    {
        public UpdateHierarchyDefinitionCommandValidator()
        {
            RuleFor(x => x.HierarchyDefinitionId).NotEmpty();

            // Stellt sicher, dass mindestens ein Feld zum Aktualisieren angegeben wird.
            RuleFor(x => x)
                .Must(x => x.Name is not null || x.RequiredBookingLevelId.HasValue)
                .WithMessage("At least one field (Name or RequiredBookingLevelId) must be provided for an update.");

            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            });
        }
    }
}
