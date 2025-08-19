using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.AnonymizeTimeEntry
{
    public class AnonymizeTimeEntryCommandValidator : AbstractValidator<AnonymizeTimeEntryCommand>
    {
        public AnonymizeTimeEntryCommandValidator()
        {
            RuleFor(x => x.TimeEntryId).NotEmpty();
        }
    }
}
