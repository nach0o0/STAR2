using Attendance.Application.Features.Commands.AssignWorkModelToEmployee;
using Attendance.Application.Features.Commands.CreateWorkModel;
using Attendance.Application.Features.Commands.DeleteWorkModel;
using Attendance.Application.Features.Commands.UnassignWorkModelFromEmployee;
using Attendance.Application.Features.Commands.UpdateEmployeeWorkModelAssignment;
using Attendance.Application.Features.Commands.UpdateWorkModel;
using Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployeeGroup;
using Attendance.Application.Features.Queries.GetWorkModelsByEmployeeGroup;
using Attendance.Contracts.Requests;
using Attendance.Contracts.Responses;
using Attendance.Domain.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Api.Controllers
{
    [ApiController]
    [Route("api/work-models")]
    [Authorize]
    public class WorkModelsController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkModelsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Policy = WorkModelPermissions.Create)]
        public async Task<IActionResult> Create(CreateWorkModelRequest request)
        {
            var command = new CreateWorkModelCommand(
                request.Name,
                request.EmployeeGroupId,
                request.MondayHours,
                request.TuesdayHours,
                request.WednesdayHours,
                request.ThursdayHours,
                request.FridayHours,
                request.SaturdayHours,
                request.SundayHours
            );

            var workModelId = await _sender.Send(command);

            var response = new CreateWorkModelResponse(workModelId);
            return StatusCode(201, response);
        }

        [HttpPut("{workModelId:guid}")]
        [Authorize(Policy = WorkModelPermissions.Update)]
        public async Task<IActionResult> Update(Guid workModelId, UpdateWorkModelRequest request)
        {
            var command = new UpdateWorkModelCommand(
                workModelId,
                request.Name,
                request.MondayHours,
                request.TuesdayHours,
                request.WednesdayHours,
                request.ThursdayHours,
                request.FridayHours,
                request.SaturdayHours,
                request.SundayHours
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpDelete("{workModelId:guid}")]
        [Authorize(Policy = WorkModelPermissions.Delete)]
        public async Task<IActionResult> Delete(Guid workModelId)
        {
            var command = new DeleteWorkModelCommand(workModelId);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPost("{workModelId:guid}/assignments/{employeeId:guid}")]
        [Authorize(Policy = WorkModelPermissions.Assign)]
        public async Task<IActionResult> AssignToEmployee(Guid workModelId, Guid employeeId, AssignWorkModelToEmployeeRequest request)
        {
            var command = new AssignWorkModelToEmployeeCommand(
                employeeId,
                workModelId,
                request.ValidFrom,
                request.ValidTo
            );

            var assignmentId = await _sender.Send(command);

            var response = new AssignWorkModelToEmployeeResponse(assignmentId);
            return StatusCode(201, response);
        }

        [HttpDelete("assignments/{assignmentId:guid}")]
        [Authorize(Policy = WorkModelPermissions.Assign)]
        public async Task<IActionResult> UnassignFromEmployee(Guid assignmentId, UnassignWorkModelFromEmployeeRequest request)
        {
            var command = new UnassignWorkModelFromEmployeeCommand(assignmentId, request.EndDate);
            await _sender.Send(command);

            return NoContent();
        }

        [HttpPut("assignments/{assignmentId:guid}")]
        [Authorize(Policy = WorkModelPermissions.Assign)]
        public async Task<IActionResult> UpdateAssignment(Guid assignmentId, UpdateEmployeeWorkModelAssignmentRequest request)
        {
            var command = new UpdateEmployeeWorkModelAssignmentCommand(
                assignmentId,
                request.ValidFrom,
                request.ValidTo
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet]
        [Authorize(Policy = WorkModelPermissions.Read)]
        public async Task<IActionResult> GetByGroup([FromQuery] Guid employeeGroupId)
        {
            var query = new GetWorkModelsByEmployeeGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(wm => new WorkModelResponse(
                wm.Id,
                wm.Name,
                wm.MondayHours,
                wm.TuesdayHours,
                wm.WednesdayHours,
                wm.ThursdayHours,
                wm.FridayHours,
                wm.SaturdayHours,
                wm.SundayHours
            ));

            return Ok(response);
        }

        [HttpGet("group/{employeeGroupId:guid}/assignments")]
        [Authorize(Policy = WorkModelPermissions.Read)]
        public async Task<IActionResult> GetAssignmentsForGroup(Guid employeeGroupId)
        {
            var query = new GetWorkModelAssignmentsForEmployeeGroupQuery(employeeGroupId);
            var result = await _sender.Send(query);

            var response = result.Select(a => new WorkModelAssignmentResponse(
                a.AssignmentId,
                a.EmployeeId, 
                a.WorkModelId,
                a.WorkModelName,
                a.ValidFrom,
                a.ValidTo
            ));

            return Ok(response);
        }
    }
}
