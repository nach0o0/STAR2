using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.UpdatePlanningEntry
{
    public class UpdatePlanningEntryCommandValidator : AbstractValidator<UpdatePlanningEntryCommand>
    {
        public UpdatePlanningEntryCommandValidator()
        {
            RuleFor(x => x.PlanningEntryId).NotEmpty();
            RuleFor(x => x.PlannedHours).GreaterThan(0).WithMessage("Planned hours must be positive.");
        }
    }
}
