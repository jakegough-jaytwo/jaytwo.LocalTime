using System;

namespace jaytwo.LocalTime;

public static class LocalTimeServiceExtensions
{
    public static DateTimeOffset AddHours(this ILocalTimeService localTimeService, DateTime localBaseTime, double hours)
        => localTimeService.Add(localBaseTime, TimeSpan.FromHours(hours));

    public static DateTimeOffset AddHours(this ILocalTimeService localTimeService, DateTime localBaseTime, double hours, bool throwOnInvalidLocalTime)
        => localTimeService.Add(localBaseTime, TimeSpan.FromHours(hours), throwOnInvalidLocalTime);

    public static DateTimeOffset AddMinutes(this ILocalTimeService localTimeService, DateTime localBaseTime, double minutes)
        => localTimeService.Add(localBaseTime, TimeSpan.FromMinutes(minutes));

    public static DateTimeOffset AddMinutes(this ILocalTimeService localTimeService, DateTime localBaseTime, double minutes, bool throwOnInvalidLocalTime)
        => localTimeService.Add(localBaseTime, TimeSpan.FromMinutes(minutes), throwOnInvalidLocalTime);

    public static DateTimeOffset AddSeconds(this ILocalTimeService localTimeService, DateTime localBaseTime, double seconds)
        => localTimeService.Add(localBaseTime, TimeSpan.FromSeconds(seconds));

    public static DateTimeOffset AddSeconds(this ILocalTimeService localTimeService, DateTime localBaseTime, double seconds, bool throwOnInvalidLocalTime)
        => localTimeService.Add(localBaseTime, TimeSpan.FromSeconds(seconds), throwOnInvalidLocalTime);

    public static DateTimeOffset Add(this ILocalTimeService localTimeService, DateTime localBaseTime, TimeSpan duration)
        => localTimeService.GetLocalDateTimeOffset(
            localTimeService.GetDateTimeOffset(localBaseTime).Add(duration));

    public static DateTimeOffset Add(this ILocalTimeService localTimeService, DateTime localBaseTime, TimeSpan duration, bool throwOnInvalidLocalTime)
        => localTimeService.GetLocalDateTimeOffset(
            localTimeService.GetDateTimeOffset(localBaseTime, throwOnInvalidLocalTime).Add(duration));

    public static DateTime GetLocalDateTime(this ILocalTimeService localTimeService, DateTimeOffset input)
        => localTimeService.GetLocalDateTimeOffset(input).DateTime;

    public static DateTime GetLocalDateTimeFromUtc(this ILocalTimeService localTimeService, DateTime utc)
        => localTimeService.GetLocalDateTime(GetDateTimeOffsetUtcFromDateTimeUtc(utc));

    public static DateTimeOffset GetLocalDateTimeOffsetFromUtc(this ILocalTimeService localTimeService, DateTime utc)
        => localTimeService.GetLocalDateTimeOffset(GetDateTimeOffsetUtcFromDateTimeUtc(utc));

    public static DateTime GetUtcDateTime(this ILocalTimeService localTimeService, DateTime local)
        => localTimeService.GetDateTimeOffset(local).UtcDateTime;

    public static DateTime GetUtcDateTime(this ILocalTimeService localTimeService, DateTime local, bool throwOnInvalidLocalTime)
        => localTimeService.GetDateTimeOffset(local, throwOnInvalidLocalTime).UtcDateTime;

    public static DateTimeOffset GetUtcDateTimeOffset(this ILocalTimeService localTimeService, DateTime local)
        => localTimeService.GetDateTimeOffset(local).ToUniversalTime();

    public static DateTimeOffset GetUtcDateTimeOffset(this ILocalTimeService localTimeService, DateTime local, bool throwOnInvalidLocalTime)
        => localTimeService.GetDateTimeOffset(local, throwOnInvalidLocalTime).ToUniversalTime();

    private static DateTimeOffset GetDateTimeOffsetUtcFromDateTimeUtc(DateTime input)
        => new DateTimeOffset(DateTime.SpecifyKind(input, DateTimeKind.Utc));
}
