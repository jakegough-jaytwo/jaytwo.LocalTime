using System;

namespace jaytwo.LocalTime;

public static class LocalTimeServiceExtensions
{
    public static DateTimeOffset AddHours(this ILocalTimeService localTimeService, DateTime localBaseTime, double hours)
        => localTimeService.Add(localBaseTime, TimeSpan.FromHours(hours));

    public static DateTimeOffset AddHours(this ILocalTimeService localTimeService, DateTime localBaseTime, double hours, bool throwOnAmbiguousOrSkipped)
        => localTimeService.Add(localBaseTime, TimeSpan.FromHours(hours), throwOnAmbiguousOrSkipped);

    public static DateTimeOffset AddMinutes(this ILocalTimeService localTimeService, DateTime localBaseTime, double minutes)
        => localTimeService.Add(localBaseTime, TimeSpan.FromMinutes(minutes));

    public static DateTimeOffset AddMinutes(this ILocalTimeService localTimeService, DateTime localBaseTime, double minutes, bool throwOnAmbiguousOrSkipped)
        => localTimeService.Add(localBaseTime, TimeSpan.FromMinutes(minutes), throwOnAmbiguousOrSkipped);

    public static DateTimeOffset AddSeconds(this ILocalTimeService localTimeService, DateTime localBaseTime, double seconds)
        => localTimeService.Add(localBaseTime, TimeSpan.FromSeconds(seconds));

    public static DateTimeOffset AddSeconds(this ILocalTimeService localTimeService, DateTime localBaseTime, double seconds, bool throwOnAmbiguousOrSkipped)
        => localTimeService.Add(localBaseTime, TimeSpan.FromSeconds(seconds), throwOnAmbiguousOrSkipped);

    public static DateTimeOffset Add(this ILocalTimeService localTimeService, DateTime localBaseTime, TimeSpan duration)
        => localTimeService.GetLocalDateTimeOffset(
            localTimeService.GetDateTimeOffset(localBaseTime).Add(duration));

    public static DateTimeOffset Add(this ILocalTimeService localTimeService, DateTime localBaseTime, TimeSpan duration, bool throwOnAmbiguousOrSkipped)
        => localTimeService.GetLocalDateTimeOffset(
            localTimeService.GetDateTimeOffset(localBaseTime, throwOnAmbiguousOrSkipped).Add(duration));

    public static DateTime GetLocalDateTime(this ILocalTimeService localTimeService, DateTimeOffset input)
        => localTimeService.GetLocalDateTimeOffset(input).DateTime;

    public static DateTime GetLocalDateTimeFromUtc(this ILocalTimeService localTimeService, DateTime utc)
        => localTimeService.GetLocalDateTime(GetDateTimeOffsetUtcFromDateTimeUtc(utc));

    public static DateTimeOffset GetLocalDateTimeOffsetFromUtc(this ILocalTimeService localTimeService, DateTime utc)
        => localTimeService.GetLocalDateTimeOffset(GetDateTimeOffsetUtcFromDateTimeUtc(utc));

    public static DateTime GetUtcDateTime(this ILocalTimeService localTimeService, DateTime local)
        => localTimeService.GetDateTimeOffset(local).UtcDateTime;

    public static DateTime GetUtcDateTime(this ILocalTimeService localTimeService, DateTime local, bool throwOnAmbiguousOrSkipped)
        => localTimeService.GetDateTimeOffset(local, throwOnAmbiguousOrSkipped).UtcDateTime;

    public static DateTimeOffset GetUtcDateTimeOffset(this ILocalTimeService localTimeService, DateTime local)
        => localTimeService.GetDateTimeOffset(local).ToUniversalTime();

    public static DateTimeOffset GetUtcDateTimeOffset(this ILocalTimeService localTimeService, DateTime local, bool throwOnAmbiguousOrSkipped)
        => localTimeService.GetDateTimeOffset(local, throwOnAmbiguousOrSkipped).ToUniversalTime();

    public static TimeSpan Subtract(this ILocalTimeService localTimeService, DateTime localBaseTime, DateTime localSubtractTime)
        => localTimeService.GetDateTimeOffset(localBaseTime)
            .Subtract(localTimeService.GetDateTimeOffset(localSubtractTime));

    public static TimeSpan Subtract(this ILocalTimeService localTimeService, DateTime localBaseTime, DateTime localSubtractTime, bool throwOnAmbiguousOrSkipped)
        => localTimeService.GetDateTimeOffset(localBaseTime, throwOnAmbiguousOrSkipped)
            .Subtract(localTimeService.GetDateTimeOffset(localSubtractTime, throwOnAmbiguousOrSkipped));

    private static DateTimeOffset GetDateTimeOffsetUtcFromDateTimeUtc(DateTime input)
        => new DateTimeOffset(DateTime.SpecifyKind(input, DateTimeKind.Utc));
}
