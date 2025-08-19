using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.UpdateTimeEntry
{
    public class UpdateTimeEntryCommandValidator : AbstractValidator<UpdateTimeEntryCommand>
    {
        public UpdateTimeEntryCommandValidator()
        {
            RuleFor(x => x.TimeEntryId).NotEmpty();

            // Es muss mindestens ein Feld zum Aktualisieren vorhanden sein.
            RuleFor(x => x)
                .Must(x => x.EntryDate.HasValue || x.CostObjectId.HasValue || x.Hours.HasValue || x.Description is not null)
                .WithMessage("At least one field must be provided for an update.");

            When(x => x.Hours.HasValue, () =>
            {
                RuleFor(x => x.Hours).GreaterThan(0).WithMessage("Hours must be positive.");
            });

            When(x => x.Description is not null, () =>
            {
                RuleFor(x => x.Description).MaximumLength(500);
            });
        }
    }
}
