using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetActiveCostObjectsByGroup
{
    public class GetActiveCostObjectsByGroupQueryHandler : IRequestHandler<GetActiveCostObjectsByGroupQuery, List<GetActiveCostObjectsByGroupQueryResult>>
    {
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly IHierarchyLevelRepository _hierarchyLevelRepository;
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly IUserContext _userContext;

        public GetActiveCostObjectsByGroupQueryHandler(
            ICostObjectRepository costObjectRepository,
            IHierarchyLevelRepository hierarchyLevelRepository,
            ICostObjectRequestRepository requestRepository,
            IUserContext userContext)
        {
            _costObjectRepository = costObjectRepository;
            _hierarchyLevelRepository = hierarchyLevelRepository;
            _requestRepository = requestRepository;
            _userContext = userContext;
        }

        public async Task<List<GetActiveCostObjectsByGroupQueryResult>> Handle(GetActiveCostObjectsByGroupQuery query, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // 1. Hole alle aktiven und genehmigten Kostenstellen für die Gruppe.
            var activeAndApprovedObjects = await _costObjectRepository.GetActiveAndApprovedByGroupAsync(query.EmployeeGroupId, cancellationToken);

            // 2. Hole alle "Pending" Anträge, die vom aktuellen Benutzer erstellt wurden.
            var myPendingRequests = await _requestRepository.GetPendingByRequesterAsync(currentUser.EmployeeId!.Value, query.EmployeeGroupId, cancellationToken);
            var myPendingCostObjectIds = myPendingRequests.Select(r => r.CostObjectId).ToHashSet();

            var myPendingObjects = new List<Domain.Entities.CostObject>();
            if (myPendingCostObjectIds.Any())
            {
                myPendingObjects = await _costObjectRepository.GetByIdsAsync(myPendingCostObjectIds, cancellationToken);
            }

            // 3. Kombiniere beide Listen und entferne Duplikate.
            var finalCostObjectList = activeAndApprovedObjects.Union(myPendingObjects).ToList();

            // 4. Lade die Hierarchie-Ebenen für die Tiefeninformation.
            var levelIds = finalCostObjectList.Select(co => co.HierarchyLevelId).Distinct().ToList();
            var levels = (await _hierarchyLevelRepository.GetByIdsAsync(levelIds, cancellationToken))
                         .ToDictionary(l => l.Id);

            return finalCostObjectList.Select(co => new GetActiveCostObjectsByGroupQueryResult(
                co.Id,
                co.Name,
                co.ParentCostObjectId,
                levels.TryGetValue(co.HierarchyLevelId, out var level) ? level.Depth : -1
            )).ToList();
        }
    }
}
