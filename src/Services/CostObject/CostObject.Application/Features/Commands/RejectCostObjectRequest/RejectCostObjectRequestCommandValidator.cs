using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.RejectCostObjectRequest
{
    public class RejectCostObjectRequestCommandValidator : AbstractValidator<RejectCostObjectRequestCommand>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly ICostObjectRequestRepository _costObjectRequestRepository;

        public RejectCostObjectRequestCommandValidator(
            ICostObjectRepository costObjectRepository,
            ICostObjectRequestRepository costObjectRequestRepository)
        {
            _costObjectRepository = costObjectRepository;
            _costObjectRequestRepository = costObjectRequestRepository;

            RuleFor(x => x.CostObjectRequestId).NotEmpty();

            RuleFor(x => x.RejectionReason)
                .NotEmpty().WithMessage("A rejection reason is required.")
                .MaximumLength(500).WithMessage("Rejection reason cannot exceed 500 characters.");

            // Diese Regel wird nur ausgeführt, wenn eine ReassignmentCostObjectId angegeben ist.
            When(x => x.ReassignmentCostObjectId.HasValue, () =>
            {
                RuleFor(x => x.ReassignmentCostObjectId)
                    .MustAsync(async (command, reassignmentId, cancellationToken) =>
                    {
                        // 1. Lade das Ziel-Kostenstellenobjekt für die Umbuchung.
                        var reassignmentCostObject = await _costObjectRepository.GetByIdAsync(reassignmentId!.Value, cancellationToken);
                        if (reassignmentCostObject == null)
                        {
                            return false; // Ziel existiert nicht.
                        }

                        // 2. Lade den ursprünglichen Antrag, um den Kontext zu erhalten.
                        var originalRequest = await _costObjectRequestRepository.GetByIdAsync(command.CostObjectRequestId, cancellationToken);
                        if (originalRequest == null)
                        {
                            return true; // Sollte nicht passieren, da die ID bereits geprüft wurde.
                        }

                        // 3. Führe die Validierungsprüfungen durch.
                        bool belongsToSameGroup = reassignmentCostObject.EmployeeGroupId == originalRequest.EmployeeGroupId;
                        bool isApprovedAndActive = reassignmentCostObject.ApprovalStatus == ApprovalStatus.Approved && (!reassignmentCostObject.ValidTo.HasValue || reassignmentCostObject.ValidTo.Value.Date >= DateTime.UtcNow.Date);
                        bool isNotSameObject = reassignmentCostObject.Id != originalRequest.CostObjectId;

                        return belongsToSameGroup && isApprovedAndActive && isNotSameObject;
                    })
                    .WithMessage("The selected cost object for reassignment is not valid. It must exist, be approved, active, and belong to the same employee group.");
            });
        }
    }
}
