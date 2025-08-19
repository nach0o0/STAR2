using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.CreatePlanningEntry
{
    public class CreatePlanningEntryCommandValidator : AbstractValidator<CreatePlanningEntryCommand>
    {
        public CreatePlanningEntryCommandValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.CostObjectId).NotEmpty();
            RuleFor(x => x.PlannedHours).GreaterThan(0).WithMessage("Planned hours must be positive.");
            RuleFor(x => x.PlanningPeriodStart).NotEmpty();
            RuleFor(x => x.PlanningPeriodEnd).NotEmpty()
                .GreaterThanOrEqualTo(x => x.PlanningPeriodStart)
                .WithMessage("End date must be on or after the start date.");
        }
    }
}
