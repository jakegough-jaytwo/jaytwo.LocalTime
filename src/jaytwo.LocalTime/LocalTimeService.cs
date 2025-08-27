using System;
using NodaTime;

namespace jaytwo.LocalTime;

public class LocalTimeService : ILocalTimeService
{
    internal const bool DefaultThrowOnInvalidLocalTime = false;

    private readonly DateTimeZone _timeZone;

    public LocalTimeService(string timeZoneId)
        : this(timeZoneId, throwOnInvalidLocalTime: DefaultThrowOnInvalidLocalTime)
    {
    }

    public LocalTimeService(string timeZoneId, bool throwOnInvalidLocalTime)
        : this(timeZoneId, throwOnInvalidLocalTime, utcNowFactory: null)
    {
    }

    private LocalTimeService(string timeZoneId, bool throwOnInvalidLocalTime, Func<DateTimeOffset>? utcNowFactory)
    {
        _timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Could not resolve time zone: '{timeZoneId}'", nameof(timeZoneId));

        ThrowOnInvalidLocalTime = throwOnInvalidLocalTime;

        UtcNowFactory = utcNowFactory ?? (static () => DateTimeOffset.UtcNow);
    }

    public Func<DateTimeOffset> UtcNowFactory { get; }

    public bool ThrowOnInvalidLocalTime { get; }

    public string TimeZoneId => _timeZone.Id;

    public DateTimeOffset UtcNow => UtcNowFactory.Invoke();

    public DateTimeOffset LocalNow => GetLocalDateTimeOffset(UtcNow);

    public static LocalTimeService Create(
        string timeZoneId,
        bool throwOnInvalidLocalTime = DefaultThrowOnInvalidLocalTime,
        Func<DateTimeOffset>? utcNowFactory = null)
        => new LocalTimeService(timeZoneId, throwOnInvalidLocalTime, utcNowFactory);

    public object HealthCheck() => HealthCheck(UtcNow);

    public DateTimeOffset GetDateTimeOffset(DateTime local)
        => GetDateTimeOffset(local, ThrowOnInvalidLocalTime);

    public DateTimeOffset GetDateTimeOffset(DateTime local, bool throwOnInvalidLocalTime = false)
        => GetZonedDateTime(local, throwOnInvalidLocalTime).ToDateTimeOffset();

    public DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input)
        => GetZonedDateTime(input).ToDateTimeOffset();

    internal ZonedDateTime GetZonedDateTime(DateTimeOffset input)
        => Instant.FromDateTimeOffset(input).InZone(_timeZone);

    internal ZonedDateTime GetZonedDateTime(DateTime input, bool throwOnInvalidLocalTime)
    {
        var localDateTime = LocalDateTime.FromDateTime(input);
        return throwOnInvalidLocalTime
            ? localDateTime.InZoneStrictly(_timeZone)
            : localDateTime.InZoneLeniently(_timeZone);
    }

    internal object HealthCheck(DateTimeOffset utcNow)
    {
        return new
        {
            TimeZoneId,
            UtcNow = utcNow,
            LocalNow = GetLocalDateTimeOffset(utcNow),
            ThrowOnInvalidLocalTime,
        };
    }
}
