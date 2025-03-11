using GarageGroup.Infra;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

internal static partial class Application
{
    private static Dependency<IDataverseApiClient> UseDataverseApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("DataverseApi")
        .UseTokenCredentialStandard()
        .UsePollyStandard()
        .UseDataverseApiClient("Dataverse");

    private static Dependency<ISqlApi> UseSqlApi()
        =>
        DataverseDbProvider.Configure("Dataverse").UseSqlApi();
}