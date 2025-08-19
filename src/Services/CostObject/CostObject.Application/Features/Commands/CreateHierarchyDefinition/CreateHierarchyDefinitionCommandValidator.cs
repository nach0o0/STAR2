using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateHierarchyDefinition
{
    public class CreateHierarchyDefinitionCommandValidator : AbstractValidator<CreateHierarchyDefinitionCommand>
    {
        public CreateHierarchyDefinitionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hierarchy definition name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.EmployeeGroupId).NotEmpty();
        }
    }
}
