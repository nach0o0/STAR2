using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.UpdateLabel
{
    public class UpdateLabelCommandValidator : AbstractValidator<UpdateLabelCommand>
    {
        public UpdateLabelCommandValidator()
        {
            RuleFor(x => x.LabelId).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Label name is required.")
                .MaximumLength(100).WithMessage("Label name cannot exceed 100 characters.");
        }
    }
}
