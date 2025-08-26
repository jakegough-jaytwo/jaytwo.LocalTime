using System;
using NodaTime;

namespace jaytwo.LocalTime;

public class LocalTimeService : ILocalTimeService
{
    private readonly DateTimeZone _timeZone;

    public LocalTimeService(string timeZoneId)
    {
        _timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Could not resolve time zone: '{timeZoneId}'", nameof(timeZoneId));
    }

    public Func<DateTimeOffset> UtcNowFactory { get; set; } = () => DateTimeOffset.UtcNow;

    public bool ThrowOnInvalidLocalTime { get; set; } = false;

    public string TimeZoneId => _timeZone.Id;

    public DateTimeOffset UtcNow => UtcNowFactory.Invoke();

    public DateTimeOffset LocalNow => GetLocalDateTimeOffset(UtcNow);

    public DateTime GetUtcDateTimeFromLocal(DateTime input)
        => GetUtcDateTimeFromLocal(input, ThrowOnInvalidLocalTime);

    public DateTime GetUtcDateTimeFromLocal(DateTime input, bool throwOnInvalidLocalTime = false)
        => GetLocalDateTimeOffset(input, throwOnInvalidLocalTime).UtcDateTime;

    public DateTimeOffset GetLocalDateTimeOffset(DateTime input)
        => GetLocalDateTimeOffset(input, ThrowOnInvalidLocalTime);

    public DateTimeOffset GetLocalDateTimeOffset(DateTime input, bool throwOnInvalidLocalTime = false)
        => GetZonedDateTime(input, throwOnInvalidLocalTime).ToDateTimeOffset();

    public DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input)
        => GetZonedDateTime(input).ToDateTimeOffset();

    public DateTimeOffset GetLocalDateTimeOffsetFromUnixTimeSeconds(long input)
        => GetLocalDateTimeOffset(DateTimeOffset.FromUnixTimeSeconds(input));

    public DateTimeOffset GetLocalDateTimeOffsetFromUnixTimeMilliseconds(long input)
        => GetLocalDateTimeOffset(DateTimeOffset.FromUnixTimeMilliseconds(input));

    public DateTime GetLocalDateTime(DateTimeOffset input)
        => GetZonedDateTime(input).ToDateTimeUnspecified();

    public DateTime GetLocalDateTimeFromUtc(DateTime input)
        => GetLocalDateTime(GetDateTimeOffsetFromUtc(input));

    public DateTime GetLocalDateTimeFromUnixTimeSeconds(long input)
        => GetLocalDateTime(DateTimeOffset.FromUnixTimeSeconds(input));

    public DateTime GetLocalDateTimeFromUnixTimeMilliseconds(long input)
        => GetLocalDateTime(DateTimeOffset.FromUnixTimeMilliseconds(input));

    internal ZonedDateTime GetZonedDateTime(DateTimeOffset input)
        => Instant.FromDateTimeOffset(input).InZone(_timeZone);

    internal ZonedDateTime GetZonedDateTime(DateTime input, bool throwOnInvalidLocalTime)
    {
        var localDateTime = LocalDateTime.FromDateTime(input);
        return throwOnInvalidLocalTime
            ? localDateTime.InZoneStrictly(_timeZone)
            : localDateTime.InZoneLeniently(_timeZone);
    }

    private static DateTimeOffset GetDateTimeOffsetFromUtc(DateTime input)
        => new DateTimeOffset(DateTime.SpecifyKind(input, DateTimeKind.Utc));
}
