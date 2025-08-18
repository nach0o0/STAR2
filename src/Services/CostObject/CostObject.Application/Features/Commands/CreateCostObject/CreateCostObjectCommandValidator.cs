using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObject
{
    public class CreateCostObjectCommandValidator : AbstractValidator<CreateCostObjectCommand>
    {
        public CreateCostObjectCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Cost object name is required.");
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.HierarchyLevelId).NotEmpty();
            RuleFor(x => x.ValidFrom).NotEmpty();
        }
    }
}
