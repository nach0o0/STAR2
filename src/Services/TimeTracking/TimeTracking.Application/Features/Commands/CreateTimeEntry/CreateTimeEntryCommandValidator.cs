using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.CreateTimeEntry
{
    public class CreateTimeEntryCommandValidator : AbstractValidator<CreateTimeEntryCommand>
    {
        public CreateTimeEntryCommandValidator()
        {
            RuleFor(x => x.EntryDate).NotEmpty();
            RuleFor(x => x.Hours).GreaterThan(0).WithMessage("Hours must be positive.");
            RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0).WithMessage("Hourly rate cannot be negative.");
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
