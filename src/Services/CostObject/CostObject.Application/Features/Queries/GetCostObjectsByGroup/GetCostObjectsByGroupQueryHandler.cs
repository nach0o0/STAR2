using CostObject.Application.Interfaces.Persistence;
using CostObject.Domain.Authorization;
using CostObject.Domain.Enums;
using MediatR;
using Shared.Application.Interfaces.Security;
using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetCostObjectsByGroup
{
    public class GetCostObjectsByGroupQueryHandler : IRequestHandler<GetCostObjectsByGroupQuery, List<GetCostObjectsByGroupQueryResult>>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly IUserContext _userContext;

        public GetCostObjectsByGroupQueryHandler(
            ICostObjectRepository costObjectRepository,
            ICostObjectRequestRepository requestRepository,
            IUserContext userContext)
        {
            _costObjectRepository = costObjectRepository;
            _requestRepository = requestRepository;
            _userContext = userContext;
        }

        public async Task<List<GetCostObjectsByGroupQueryResult>> Handle(GetCostObjectsByGroupQuery query, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;
            var allObjectsInGroup = await _costObjectRepository.GetForGroupWithRequestsAsync(query.EmployeeGroupId, cancellationToken);

            var requiredPermission = CostObjectPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{query.EmployeeGroupId}";
            var userHasReadPermission = currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) && permissions.Contains(requiredPermission);

            var filteredList = new List<Domain.Entities.CostObject>();

            // Lade alle Anfragen für diese Gruppe, um den Ersteller zu kennen
            var requests = (await _requestRepository.GetByGroupIdAsync(query.EmployeeGroupId, cancellationToken))
                           .ToDictionary(r => r.CostObjectId);

            foreach (var costObject in allObjectsInGroup)
            {
                // Regel 1: Genehmigte Kostenstellen werden immer angezeigt.
                if (costObject.ApprovalStatus == ApprovalStatus.Approved)
                {
                    filteredList.Add(costObject);
                    continue;
                }

                // Regel 2: Wenn der Benutzer die Leseberechtigung hat, werden alle Pending-Objekte angezeigt.
                if (costObject.ApprovalStatus == ApprovalStatus.Pending && userHasReadPermission)
                {
                    filteredList.Add(costObject);
                    continue;
                }

                // Regel 3: Wenn der Benutzer keine Leseberechtigung hat, werden Pending-Objekte nur angezeigt, wenn er der Antragsteller ist.
                if (costObject.ApprovalStatus == ApprovalStatus.Pending && !userHasReadPermission)
                {
                    if (requests.TryGetValue(costObject.Id, out var request) && request.RequesterEmployeeId == currentUser.EmployeeId)
                    {
                        filteredList.Add(costObject);
                    }
                }
            }

            return filteredList.Select(co => new GetCostObjectsByGroupQueryResult(
                co.Id, co.Name, co.EmployeeGroupId, co.ParentCostObjectId,
                co.HierarchyLevelId, co.LabelId, co.ValidFrom, co.ValidTo, co.ApprovalStatus
            )).ToList();
        }
    }
}
