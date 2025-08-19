using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.DeletePlanningEntry
{
    public class DeletePlanningEntryCommandValidator : AbstractValidator<DeletePlanningEntryCommand>
    {
        public DeletePlanningEntryCommandValidator()
        {
            RuleFor(x => x.PlanningEntryId).NotEmpty();
        }
    }
}
