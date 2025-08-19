using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObjectRequest
{
    public class CreateCostObjectRequestCommandValidator : AbstractValidator<CreateCostObjectRequestCommand>
    {
        public CreateCostObjectRequestCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Cost object name is required.");
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.HierarchyLevelId).NotEmpty();
            RuleFor(x => x.ValidFrom).NotEmpty();
        }
    }
}
