using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetHourlyRateById
{
    public class GetHourlyRateByIdQueryValidator : AbstractValidator<GetHourlyRateByIdQuery>
    {
        public GetHourlyRateByIdQueryValidator()
        {
            RuleFor(x => x.HourlyRateId).NotEmpty();
        }
    }
}
