using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Application.Features.Queries.ListMySessions
{
    public class ListMySessionsQueryValidator : AbstractValidator<ListMySessionsQuery>
    {
        public ListMySessionsQueryValidator()
        {
            // Keine Validierungsregeln notwendig, da der Query keine Parameter hat.
        }
    }
}
