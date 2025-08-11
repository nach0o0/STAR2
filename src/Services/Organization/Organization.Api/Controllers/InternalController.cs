using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Features.Queries.GetEmployeeInfoByUserId;
using Organization.Application.Features.Queries.GetEmployeesByUserIds;
using Organization.Contracts.Requests;
using Organization.Contracts.Responses;

namespace Organization.Api.Controllers
{
    [ApiController]
    [Route("api/internal")]
    public class InternalController : ControllerBase
    {
        private readonly ISender _sender;

        public InternalController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("employee-info/by-user/{userId}")]
        public async Task<IActionResult> GetEmployeeInfoByUserId(Guid userId)
        {
            var query = new GetEmployeeInfoByUserIdQuery(userId);
            var queryResult = await _sender.Send(query);

            if (queryResult is null)
            {
                return NotFound();
            }

            var response = new EmployeeInfoResponse(
                queryResult.EmployeeId,
                queryResult.OrganizationId,
                queryResult.EmployeeGroupIds);

            return Ok(response);
        }

        [HttpPost("employees/by-user-ids")]
        public async Task<IActionResult> GetEmployeesByUserIds(GetEmployeesByUserIdsRequest request)
        {
            var query = new GetEmployeesByUserIdsQuery(request.UserIds);
            var result = await _sender.Send(query);

            var response = result.Select(e => new EmployeeDetailsResponse(
                e.UserId, e.EmployeeId, e.FirstName, e.LastName
            )).ToList();

            return Ok(response);
        }
    }
}
