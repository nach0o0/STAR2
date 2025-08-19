using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.RejectCostObjectRequest
{
    public class RejectCostObjectRequestCommandHandler : IRequestHandler<RejectCostObjectRequestCommand>
    {
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly IUserContext _userContext;

        public RejectCostObjectRequestCommandHandler(
            ICostObjectRequestRepository requestRepository,
            IUserContext userContext)
        {
            _requestRepository = requestRepository;
            _userContext = userContext;
        }

        public async Task Handle(RejectCostObjectRequestCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            var request = await _requestRepository.GetByIdAsync(command.CostObjectRequestId, cancellationToken);
            if (request is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObjectRequest), command.CostObjectRequestId);
            }

            // Logik zum Prüfen, ob die Reassignment-Kostenstelle existiert und gültig ist,
            // könnte hier oder im Validator hinzugefügt werden.

            request.Reject(
                currentUser.EmployeeId!.Value,
                command.RejectionReason,
                command.ReassignmentCostObjectId // Übergabe der neuen ID
            );
        }
    }
}
