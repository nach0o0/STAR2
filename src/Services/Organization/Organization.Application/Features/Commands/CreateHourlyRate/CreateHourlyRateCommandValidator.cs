using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateHourlyRate
{
    public class CreateHourlyRateCommandValidator : AbstractValidator<CreateHourlyRateCommand>
    {
        public CreateHourlyRateCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Rate).GreaterThan(0);
            RuleFor(x => x.OrganizationId).NotEmpty();
            RuleFor(x => x.ValidFrom).NotEmpty();
        }
    }
}
