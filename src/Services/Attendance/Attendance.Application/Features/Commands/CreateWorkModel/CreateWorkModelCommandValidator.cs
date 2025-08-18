using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateWorkModel
{
    public class CreateWorkModelCommandValidator : AbstractValidator<CreateWorkModelCommand>
    {
        public CreateWorkModelCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EmployeeGroupId).NotEmpty();

            RuleFor(x => x.MondayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.TuesdayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.WednesdayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.ThursdayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.FridayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.SaturdayHours).InclusiveBetween(0, 24);
            RuleFor(x => x.SundayHours).InclusiveBetween(0, 24);
        }
    }
}
