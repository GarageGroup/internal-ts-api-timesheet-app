using GarageGroup.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Internal.Timesheet;

using static TimesheetDeleteMetadata;

[Endpoint(EndpointMethod.Delete, Func.Route, Summary = Func.Summary, Description = Func.Description)]
[EndpointTag(Func.Tag)]
public interface ITimesheetDeleteFunc
{
    ValueTask<Result<Unit, Failure<Unit>>> InvokeAsync(
        TimesheetDeleteIn input, CancellationToken cancellationToken);
}