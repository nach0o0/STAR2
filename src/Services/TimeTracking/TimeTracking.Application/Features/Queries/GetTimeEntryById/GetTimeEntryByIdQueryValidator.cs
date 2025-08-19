using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntryById
{
    public class GetTimeEntryByIdQueryValidator : AbstractValidator<GetTimeEntryByIdQuery>
    {
        public GetTimeEntryByIdQueryValidator()
        {
            RuleFor(x => x.TimeEntryId).NotEmpty();
        }
    }
}
