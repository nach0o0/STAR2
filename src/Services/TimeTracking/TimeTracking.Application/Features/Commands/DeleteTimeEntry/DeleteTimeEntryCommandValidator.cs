using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Commands.DeleteTimeEntry
{
    public class DeleteTimeEntryCommandValidator : AbstractValidator<DeleteTimeEntryCommand>
    {
        public DeleteTimeEntryCommandValidator()
        {
            RuleFor(x => x.TimeEntryId).NotEmpty();
        }
    }
}
