using System;
using System.Text.Json;
using GarageGroup.Infra;

namespace GarageGroup.Internal.Timesheet;

internal sealed partial class NotificationSubscribeFunc : INotificationSubscribeFunc, INotificationUnsubscribeFunc
{
    private static readonly JsonSerializerOptions SerializerOptions
        =
        new(JsonSerializerDefaults.Web);

    private readonly IDataverseApiClient dataverseApi;

    private readonly NotificationSubscribeOption option;

    internal NotificationSubscribeFunc(IDataverseApiClient dataverseApi, NotificationSubscribeOption option)
    {
        this.dataverseApi = dataverseApi;
        this.option = option;
    }

    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        NotificationSubscribeIn input)
    {
        if (input.SubscriptionData.UserPreference is null)
        {
            return new NotificationSubscriptionJson();
        }
        
        return input.SubscriptionData.UserPreference switch
        {
            DailyNotificationUserPreference userPreference => ValidateAndMapToJsonDto(userPreference),
            WeeklyNotificationUserPreference userPreference => ValidateAndMapToJsonDto(userPreference),
            _ => Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Unexpected type of user preferences")
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        DailyNotificationUserPreference userPreference)
    {
        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Daily working hours cannot be less than zero");
        }

        var jsonUserPreferences = DailyNotificationUserPreferencesJson.Parse(userPreference);
        var userPreferences = JsonSerializer.Serialize(jsonUserPreferences, SerializerOptions);

        return new NotificationSubscriptionJson
        {
            UserPreferences = userPreferences
        };
    }
    
    private static Result<NotificationSubscriptionJson, Failure<NotificationSubscribeFailureCode>> ValidateAndMapToJsonDto(
        WeeklyNotificationUserPreference userPreference)
    {
        if (userPreference.Weekday.IsEmpty)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Weekdays for notifications must be specified");
        }

        if (userPreference.WorkedHours <= 0)
        {
            return Failure.Create(NotificationSubscribeFailureCode.InvalidQuery, "Total week working hours cannot be less than zero");
        }

        var jsonUserPreferences = WeeklyNotificationUserPreferencesJson.Parse(userPreference);
        var userPreferences = JsonSerializer.Serialize(jsonUserPreferences, SerializerOptions);

        return new NotificationSubscriptionJson
        {
            UserPreferences = userPreferences
        };
    }

    private static Result<string, Failure<NotificationSubscribeFailureCode>> MapToNotificationTypeKey(NotificationSubscribeIn input) 
        => 
        MapToNotificationTypeKey(input.SubscriptionData.NotificationType);

    private static Result<string, Failure<NotificationSubscribeFailureCode>> MapToNotificationTypeKey(NotificationType type)
        =>
        type switch
        {
            NotificationType.DailyNotification => "dailyTimesheetNotification",
            NotificationType.WeeklyNotification => "weeklyTimesheetNotification",
            _ => Failure.Create(NotificationSubscribeFailureCode.NotificationTypeInvalid, "Not supported type of subscription data")
        };

    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingBotUser(DataverseFailureCode failureCode) 
        => 
        failureCode switch
        {
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.BotUserNotFound,
            _ => NotificationSubscribeFailureCode.Unknown 
        };

    private static NotificationSubscribeFailureCode MapFailureCodeWhenFindingNotificationType(DataverseFailureCode failureCode)
        => 
        failureCode switch
        { 
            DataverseFailureCode.RecordNotFound => NotificationSubscribeFailureCode.NotificationTypeNotFound, 
            _ => NotificationSubscribeFailureCode.Unknown
        };

    private sealed record class NotificationData(NotificationSubscribeIn Input, NotificationSubscriptionJson Subscription);
}
