using CostObject.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateLabel
{
    public class CreateLabelCommandValidator : AbstractValidator<CreateLabelCommand>
    {
        public CreateLabelCommandValidator(ILabelRepository labelRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Label name is required.")
                .MaximumLength(100).WithMessage("Label name cannot exceed 100 characters.");

            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
