using GarageGroup.Infra;
using System;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetDeleteMetadata;

public readonly record struct TimesheetDeleteIn
{
    public TimesheetDeleteIn(
        [RouteIn, SwaggerDescription(In.IdDescription), StringExample(In.IdExample)] Guid timesheetId)
        =>
        TimesheetId = timesheetId;

    public Guid TimesheetId { get; }
}