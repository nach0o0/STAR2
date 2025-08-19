using CostObject.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateHierarchyLevel
{
    public class CreateHierarchyLevelCommandValidator : AbstractValidator<CreateHierarchyLevelCommand>
    {
        public CreateHierarchyLevelCommandValidator(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Level name is required.")
                .MaximumLength(100).WithMessage("Level name cannot exceed 100 characters.");

            RuleFor(x => x.HierarchyDefinitionId).NotEmpty();

            RuleFor(x => x.Depth)
                .GreaterThanOrEqualTo(0).WithMessage("Depth must be a positive integer.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                    !await hierarchyLevelRepository.DepthExistsInHierarchyAsync(command.Depth, command.HierarchyDefinitionId, cancellation))
                .WithMessage("A level with this depth already exists in this hierarchy definition.");
        }
    }
}
