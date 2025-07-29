using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.UpdateHourlyRate
{
    public class UpdateHourlyRateCommandValidator : AbstractValidator<UpdateHourlyRateCommand>
    {
        public UpdateHourlyRateCommandValidator()
        {
            RuleFor(x => x.HourlyRateId).NotEmpty();

            RuleFor(x => x.Rate)
                .GreaterThan(0)
                .When(x => x.Rate.HasValue);
        }
    }
}
