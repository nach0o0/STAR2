using Attendance.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployeeGroup
{
    public class GetWorkModelAssignmentsForEmployeeGroupQueryHandler
        : IRequestHandler<GetWorkModelAssignmentsForEmployeeGroupQuery, List<GetWorkModelAssignmentsForEmployeeGroupQueryResult>>
    {
        private readonly IEmployeeWorkModelRepository _assignmentRepository;
        private readonly IWorkModelRepository _workModelRepository;

        public GetWorkModelAssignmentsForEmployeeGroupQueryHandler(
            IEmployeeWorkModelRepository assignmentRepository,
            IWorkModelRepository workModelRepository)
        {
            _assignmentRepository = assignmentRepository;
            _workModelRepository = workModelRepository;
        }

        public async Task<List<GetWorkModelAssignmentsForEmployeeGroupQueryResult>> Handle(
            GetWorkModelAssignmentsForEmployeeGroupQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Finde alle Arbeitsmodelle, die zu dieser Gruppe gehören.
            var workModelsInGroup = await _workModelRepository.GetByEmployeeGroupIdAsync(request.EmployeeGroupId, cancellationToken);
            if (!workModelsInGroup.Any())
            {
                return new List<GetWorkModelAssignmentsForEmployeeGroupQueryResult>();
            }

            // 2. Finde alle Zuweisungen, die diese Arbeitsmodelle verwenden.
            var workModelIds = workModelsInGroup.Select(wm => wm.Id);
            var assignments = await _assignmentRepository.GetAssignmentsByWorkModelIdsAsync(workModelIds, cancellationToken);

            // 3. Wandle die Ergebnisse in das DTO um.
            var workModelDict = workModelsInGroup.ToDictionary(wm => wm.Id);
            return assignments.Select(a => new GetWorkModelAssignmentsForEmployeeGroupQueryResult(
                a.Id,
                a.EmployeeId,
                a.WorkModelId,
                workModelDict.ContainsKey(a.WorkModelId) ? workModelDict[a.WorkModelId].Name : "Unknown",
                a.ValidFrom,
                a.ValidTo
            )).ToList();
        }
    }
}
