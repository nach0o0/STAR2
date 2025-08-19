using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetAnonymizedTimeEntry
{
    public class GetAnonymizedTimeEntryQueryValidator : AbstractValidator<GetAnonymizedTimeEntryQuery>
    {
        public GetAnonymizedTimeEntryQueryValidator()
        {
            RuleFor(x => x.AccessKey).NotEmpty();
        }
    }
}
