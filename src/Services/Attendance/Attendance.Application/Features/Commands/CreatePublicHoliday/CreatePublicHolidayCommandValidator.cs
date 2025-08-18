using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreatePublicHoliday
{
    public class CreatePublicHolidayCommandValidator : AbstractValidator<CreatePublicHolidayCommand>
    {
        public CreatePublicHolidayCommandValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
