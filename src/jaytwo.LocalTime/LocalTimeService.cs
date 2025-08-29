using System;
using NodaTime;

namespace jaytwo.LocalTime;

public class LocalTimeService : ILocalTimeService
{
    internal const bool DefaultThrowOnAmbiguousOrSkipped = false;

    private readonly DateTimeZone _timeZone;

    public LocalTimeService(string timeZoneId)
        : this(timeZoneId, throwOnAmbiguousOrSkipped: DefaultThrowOnAmbiguousOrSkipped)
    {
    }

    public LocalTimeService(string timeZoneId, bool throwOnAmbiguousOrSkipped)
        : this(timeZoneId, throwOnAmbiguousOrSkipped, utcNowFactory: null)
    {
    }

    private LocalTimeService(string timeZoneId, bool throwOnAmbiguousOrSkipped, Func<DateTimeOffset>? utcNowFactory)
    {
        _timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Could not resolve time zone: '{timeZoneId}'", nameof(timeZoneId));

        ThrowOnAmbiguousOrSkipped = throwOnAmbiguousOrSkipped;

        UtcNowFactory = utcNowFactory ?? (static () => DateTimeOffset.UtcNow);
    }

    public Func<DateTimeOffset> UtcNowFactory { get; }

    public bool ThrowOnAmbiguousOrSkipped { get; }

    public string TimeZoneId => _timeZone.Id;

    public DateTimeOffset UtcNow => UtcNowFactory.Invoke();

    public DateTimeOffset LocalNow => GetLocalDateTimeOffset(UtcNow);

    public static LocalTimeService Create(
        string timeZoneId,
        bool throwOnAmbiguousOrSkipped = DefaultThrowOnAmbiguousOrSkipped,
        Func<DateTimeOffset>? utcNowFactory = null)
        => new LocalTimeService(timeZoneId, throwOnAmbiguousOrSkipped, utcNowFactory);

    public object HealthCheck() => HealthCheck(UtcNow);

    public DateTimeOffset GetDateTimeOffset(DateTime local)
        => GetDateTimeOffset(local, ThrowOnAmbiguousOrSkipped);

    public DateTimeOffset GetDateTimeOffset(DateTime local, bool throwOnAmbiguousOrSkipped)
        => GetZonedDateTime(local, throwOnAmbiguousOrSkipped).ToDateTimeOffset();

    public DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input)
        => GetZonedDateTime(input).ToDateTimeOffset();

    internal ZonedDateTime GetZonedDateTime(DateTimeOffset input)
        => Instant.FromDateTimeOffset(input).InZone(_timeZone);

    internal ZonedDateTime GetZonedDateTime(DateTime input, bool throwOnAmbiguousOrSkipped)
    {
        var localDateTime = LocalDateTime.FromDateTime(input);
        return throwOnAmbiguousOrSkipped
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
            ThrowOnAmbiguousOrSkipped,
        };
    }
}
