using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.ApproveCostObjectRequest
{
    public class ApproveCostObjectRequestCommandHandler : IRequestHandler<ApproveCostObjectRequestCommand>
    {
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly IUserContext _userContext;

        public ApproveCostObjectRequestCommandHandler(
            ICostObjectRequestRepository requestRepository,
            ICostObjectRepository costObjectRepository,
            IUserContext userContext)
        {
            _requestRepository = requestRepository;
            _costObjectRepository = costObjectRepository;
            _userContext = userContext;
        }

        public async Task Handle(ApproveCostObjectRequestCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            var request = await _requestRepository.GetByIdAsync(command.CostObjectRequestId, cancellationToken);
            if (request is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObjectRequest), command.CostObjectRequestId);
            }

            var costObject = await _costObjectRepository.GetByIdAsync(request.CostObjectId, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), request.CostObjectId);
            }

            // 1. Genehmige den Antrag
            request.Approve(currentUser.EmployeeId!.Value);

            // 2. Genehmige die Kostenstelle selbst
            costObject.Approve();
        }
    }
}
