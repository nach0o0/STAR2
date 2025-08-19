using CostObject.Application.Interfaces.Persistence;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.DeleteHierarchyDefinition
{
    public class DeleteHierarchyDefinitionCommandValidator : AbstractValidator<DeleteHierarchyDefinitionCommand>
    {
        public DeleteHierarchyDefinitionCommandValidator(IHierarchyLevelRepository hierarchyLevelRepository)
        {
            RuleFor(x => x.HierarchyDefinitionId).NotEmpty();

            // Geschäftsregel: Eine Definition kann nicht gelöscht werden, wenn sie noch Ebenen hat.
            RuleFor(x => x.HierarchyDefinitionId)
                .MustAsync(async (definitionId, cancellationToken) =>
                    !await hierarchyLevelRepository.IsHierarchyDefinitionInUseAsync(definitionId, cancellationToken))
                .WithMessage("This hierarchy definition cannot be deleted because it still contains hierarchy levels.");
        }
    }
}
