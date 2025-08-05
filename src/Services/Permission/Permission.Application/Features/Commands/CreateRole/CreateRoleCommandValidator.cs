using FluentValidation;
using Permission.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Commands.CreateRole
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(IRoleRepository roleRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MustAsync(async (command, name, cancellation) =>
                    !await roleRepository.NameExistsInScopeAsync(name, command.Scope, cancellation))
                .WithMessage("A role with this name already exists in this scope.");

            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
