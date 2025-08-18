using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateWorkModel
{
    public class UpdateWorkModelCommandValidator : AbstractValidator<UpdateWorkModelCommand>
    {
        public UpdateWorkModelCommandValidator()
        {
            RuleFor(x => x.WorkModelId).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(100);

            // Stellt sicher, dass WENN ein Wert angegeben wird, dieser im gültigen Bereich liegt.
            RuleFor(x => x.MondayHours).InclusiveBetween(0, 24).When(x => x.MondayHours.HasValue);
            RuleFor(x => x.TuesdayHours).InclusiveBetween(0, 24).When(x => x.TuesdayHours.HasValue);
            RuleFor(x => x.WednesdayHours).InclusiveBetween(0, 24).When(x => x.WednesdayHours.HasValue);
            RuleFor(x => x.ThursdayHours).InclusiveBetween(0, 24).When(x => x.ThursdayHours.HasValue);
            RuleFor(x => x.FridayHours).InclusiveBetween(0, 24).When(x => x.FridayHours.HasValue);
            RuleFor(x => x.SaturdayHours).InclusiveBetween(0, 24).When(x => x.SaturdayHours.HasValue);
            RuleFor(x => x.SundayHours).InclusiveBetween(0, 24).When(x => x.SundayHours.HasValue);
        }
    }
}
