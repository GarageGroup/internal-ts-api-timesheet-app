namespace GarageGroup.Internal.Timesheet;

internal static class TimesheetDeleteMetadata
{
    public static class Func
    {
        public const string Tag = "Timesheet";

        public const string Route = "/timesheet/{timesheetId}";

        public const string Summary
            =
            "Delete timesheet";

        public const string Description
            =
            "Deletes the specified timesheet.";
    }

    public static class In
    {
        public const string IdDescription
            =
            "Unique identifier in Dataverse.";

        public const string IdExample = "cec83895-a5c2-4073-89d5-4691e3710ceb";
    }
}