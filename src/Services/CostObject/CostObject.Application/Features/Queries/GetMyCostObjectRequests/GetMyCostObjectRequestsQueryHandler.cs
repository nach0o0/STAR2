using CostObject.Application.Interfaces.Persistence;
using MediatR;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Queries.GetMyCostObjectRequests
{
    public class GetMyCostObjectRequestsQueryHandler : IRequestHandler<GetMyCostObjectRequestsQuery, List<GetMyCostObjectRequestsQueryResult>>
    {
        private readonly ICostObjectRequestRepository _requestRepository;
        private readonly ICostObjectRepository _costObjectRepository;
        private readonly IUserContext _userContext;

        public GetMyCostObjectRequestsQueryHandler(
            ICostObjectRequestRepository requestRepository,
            ICostObjectRepository costObjectRepository,
            IUserContext userContext)
        {
            _requestRepository = requestRepository;
            _costObjectRepository = costObjectRepository;
            _userContext = userContext;
        }

        public async Task<List<GetMyCostObjectRequestsQueryResult>> Handle(GetMyCostObjectRequestsQuery query, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // 1. Hole alle Anträge, die vom aktuellen Benutzer in der spezifizierten Gruppe gestellt wurden.
            var myRequests = await _requestRepository.GetByRequesterAndGroupAsync(currentUser.EmployeeId!.Value, query.EmployeeGroupId, cancellationToken);

            if (!myRequests.Any())
            {
                return new List<GetMyCostObjectRequestsQueryResult>();
            }

            // 2. Lade die zugehörigen Kostenstellen-Namen
            var costObjectIds = myRequests.Select(r => r.CostObjectId).Distinct().ToList();
            var costObjects = (await _costObjectRepository.GetByIdsAsync(costObjectIds, cancellationToken))
                              .ToDictionary(co => co.Id);

            return myRequests.Select(r => new GetMyCostObjectRequestsQueryResult(
                r.Id,
                r.CostObjectId,
                costObjects.TryGetValue(r.CostObjectId, out var co) ? co.Name : "N/A",
                r.Status,
                r.CreatedAt,
                r.RejectionReason
            )).OrderByDescending(r => r.CreatedAt).ToList();
        }
    }
}
