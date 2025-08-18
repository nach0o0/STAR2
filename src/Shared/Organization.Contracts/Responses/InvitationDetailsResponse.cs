using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Organization.Contracts.Responses
{
    public record InvitationDetailsResponse(
        Guid InvitationId,
        Guid InviterEmployeeId,
        Guid InviteeEmployeeId,
        InvitationTargetEntityType TargetType,
        Guid TargetId,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        InvitationStatusDto Status,
        DateTime ExpiresAt,
        DateTime CreatedAt
    );
}
