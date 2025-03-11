using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetSetGetMetadata;

[Endpoint(EndpointMethod.Get, Func.Route, Summary = Func.Summary, Description = Func.Description)]
[EndpointTag(Func.Tag)]
public interface ITimesheetSetGetFunc
{
    ValueTask<Result<TimesheetSetGetOut, Failure<Unit>>> InvokeAsync(
        TimesheetSetGetIn input, CancellationToken cancellationToken);
}