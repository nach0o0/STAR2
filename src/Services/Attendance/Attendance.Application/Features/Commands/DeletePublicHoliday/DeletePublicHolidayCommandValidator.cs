using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeletePublicHoliday
{
    public class DeletePublicHolidayCommandValidator : AbstractValidator<DeletePublicHolidayCommand>
    {
        public DeletePublicHolidayCommandValidator()
        {
            RuleFor(x => x.PublicHolidayId).NotEmpty();
        }
    }
}
