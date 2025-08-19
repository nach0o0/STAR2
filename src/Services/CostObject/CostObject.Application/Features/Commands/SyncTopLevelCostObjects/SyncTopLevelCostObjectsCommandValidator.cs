using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.SyncTopLevelCostObjects
{
    public class SyncTopLevelCostObjectsCommandValidator : AbstractValidator<SyncTopLevelCostObjectsCommand>
    {
        public SyncTopLevelCostObjectsCommandValidator()
        {
            RuleFor(x => x.EmployeeGroupId).NotEmpty();
            RuleFor(x => x.Names).NotNull();
            RuleFor(x => x.ValidFrom).NotEmpty();
            RuleFor(x => x.TopHierarchyLevelId).NotEmpty();
        }
    }
}
