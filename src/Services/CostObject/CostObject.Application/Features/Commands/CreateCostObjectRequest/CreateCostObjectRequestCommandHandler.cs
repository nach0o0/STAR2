using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Entities;
using MediatR;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateCostObjectRequest
{
    public class CreateCostObjectRequestCommandHandler : IRequestHandler<CreateCostObjectRequestCommand, Guid>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly ICostObjectRequestRepository _costObjectRequestRepository;
        private readonly IUserContext _userContext;

        public CreateCostObjectRequestCommandHandler(
            ICostObjectRepository costObjectRepository,
            ICostObjectRequestRepository costObjectRequestRepository,
            IUserContext userContext)
        {
            _costObjectRepository = costObjectRepository;
            _costObjectRequestRepository = costObjectRequestRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreateCostObjectRequestCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // 1. Erstelle die neue Kostenstelle, aber mit dem Status "Pending", da sie noch nicht genehmigt ist.
            var newCostObject = new Domain.Entities.CostObject(
                command.Name,
                command.EmployeeGroupId,
                command.ParentCostObjectId,
                command.HierarchyLevelId,
                command.LabelId,
                command.ValidFrom,
                isApprovedDirectly: false // Wichtig: Ist nicht direkt genehmigt
            );

            await _costObjectRepository.AddAsync(newCostObject, cancellationToken);

            // 2. Erstelle den dazugehörigen Antrag (Request).
            var costObjectRequest = new CostObjectRequest(
                newCostObject.Id,
                currentUser.EmployeeId!.Value, // Authorizer stellt sicher, dass ID vorhanden ist
                command.EmployeeGroupId
            );

            await _costObjectRequestRepository.AddAsync(costObjectRequest, cancellationToken);

            // Die UnitOfWork-Pipeline speichert beide neuen Entitäten.
            return costObjectRequest.Id;
        }
    }
}
