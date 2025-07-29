using FluentValidation;
using Organization.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteEmployeeGroup
{
    public class DeleteEmployeeGroupCommandValidator : AbstractValidator<DeleteEmployeeGroupCommand>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public DeleteEmployeeGroupCommandValidator(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;

            RuleFor(x => x.EmployeeGroupId)
                .NotEmpty()
                .MustAsync(async (groupId, cancellation) =>
                    !await _organizationRepository.IsDefaultGroupOfAnyOrganizationAsync(groupId, cancellation))
                .WithMessage("The default employee group of an organization cannot be deleted.");
        }
    }
}
