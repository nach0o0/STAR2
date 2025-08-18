using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateCostObject
{
    public class UpdateCostObjectCommandValidator : AbstractValidator<UpdateCostObjectCommand>
    {
        public UpdateCostObjectCommandValidator()
        {
            RuleFor(x => x.CostObjectId).NotEmpty();

            // Stellt sicher, dass mindestens ein Feld zum Aktualisieren angegeben wird.
            RuleFor(x => x)
                .Must(x => x.Name is not null || x.ParentCostObjectId.HasValue || x.HierarchyLevelId.HasValue || x.LabelId.HasValue)
                .WithMessage("At least one field must be provided for an update.");
        }
    }
}
