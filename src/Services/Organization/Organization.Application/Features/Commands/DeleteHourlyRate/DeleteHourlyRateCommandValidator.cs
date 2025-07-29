using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteHourlyRate
{
    public class DeleteHourlyRateCommandValidator : AbstractValidator<DeleteHourlyRateCommand>
    {
        public DeleteHourlyRateCommandValidator()
        {
            RuleFor(x => x.HourlyRateId).NotEmpty();
        }
    }
}
