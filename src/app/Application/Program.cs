using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace GarageGroup.Internal.Timesheet;

static class Program
{
    static Task Main(string[] args)
        =>
        AzureApplication.Create(args)
        .UseHealthCheck()
        .UseSwagger()
        .UseStandardSwaggerUI()
        .UseJwtReader()
        .UseTimesheetSetGetEndpoint()
        .UseTimesheetCreateEndpoint()
        .UseTimesheetDeleteEndpoint()
        .UseTimesheetUpdateEndpoint()
        .RunAsync();
}