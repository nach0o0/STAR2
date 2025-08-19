using CostObject.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteHierarchyLevel
{
    public class DeleteHierarchyLevelCommandValidator : AbstractValidator<DeleteHierarchyLevelCommand>
    {
        public DeleteHierarchyLevelCommandValidator(ICostObjectRepository costObjectRepository)
        {
            RuleFor(x => x.HierarchyLevelId).NotEmpty();

            // Geschäftsregel: Eine Ebene kann nicht gelöscht werden, wenn sie noch in Verwendung ist.
            RuleFor(x => x.HierarchyLevelId)
                .MustAsync(async (levelId, cancellationToken) =>
                    !await costObjectRepository.IsHierarchyLevelInUseAsync(levelId, cancellationToken))
                .WithMessage("This hierarchy level cannot be deleted because it is still assigned to one or more cost objects.");
        }
    }
}
