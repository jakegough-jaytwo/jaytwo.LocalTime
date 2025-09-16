using System;
using jaytwo.Rounding;
using NodaTime;

namespace jaytwo.LocalTime;

public class LocalTimeService : ILocalTimeService
{
    internal const bool DefaultThrowOnAmbiguousOrSkipped = false;
    internal const TimePrecision DefaultNowPrecision = TimePrecision.None;

    private readonly DateTimeZone _timeZone;

    public LocalTimeService(
        string timeZoneId,
        bool throwOnAmbiguousOrSkipped = DefaultThrowOnAmbiguousOrSkipped,
        TimePrecision nowPrecision = DefaultNowPrecision,
        Func<DateTimeOffset>? utcNowFactory = null)
    {
        _timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId)
            ?? throw new ArgumentException($"Could not resolve time zone: '{timeZoneId}'", nameof(timeZoneId));

        ThrowOnAmbiguousOrSkipped = throwOnAmbiguousOrSkipped;

        NowPrecision = nowPrecision;

        UtcNowFactory = utcNowFactory ?? (static () => DateTimeOffset.UtcNow);
    }

    public Func<DateTimeOffset> UtcNowFactory { get; }

    public bool ThrowOnAmbiguousOrSkipped { get; }

    public TimePrecision NowPrecision { get; }

    public string TimeZoneId => _timeZone.Id;

    public DateTimeOffset UtcNow => Truncate(UtcNowFactory.Invoke(), NowPrecision);

    public DateTimeOffset LocalNow => GetLocalDateTimeOffset(UtcNow);

    public object HealthCheck() => HealthCheck(UtcNow);

    public DateTimeOffset GetDateTimeOffset(DateTime local)
        => GetDateTimeOffset(local, ThrowOnAmbiguousOrSkipped);

    public DateTimeOffset GetDateTimeOffset(DateTime local, bool throwOnAmbiguousOrSkipped)
        => GetZonedDateTime(local, throwOnAmbiguousOrSkipped).ToDateTimeOffset();

    public DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input)
        => GetZonedDateTime(input).ToDateTimeOffset();

    internal static DateTimeOffset Truncate(DateTimeOffset input, TimePrecision truncation)
    {
        if (truncation == TimePrecision.None)
        {
            return input;
        }
        else
        {
            var precision = GetTimePrecision(truncation);
            return TimeQuantizer.Quantize(input, precision, QuantizationMode.Truncate);
        }
    }

    internal ZonedDateTime GetZonedDateTime(DateTimeOffset input)
    {
        var instant = Instant.FromDateTimeOffset(input);
        return instant.InZone(_timeZone);
    }

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
            TimestampResolution = NowPrecision.ToString("G"),
            ThrowOnAmbiguousOrSkipped,
        };
    }

    private static Rounding.TimePrecision GetTimePrecision(TimePrecision truncation)
    {
        return truncation switch
        {
            TimePrecision.Microsecond => Rounding.TimePrecision.Microsecond,
            TimePrecision.Millisecond => Rounding.TimePrecision.Millisecond,
            TimePrecision.Second => Rounding.TimePrecision.Second,
            TimePrecision.Minute => Rounding.TimePrecision.Minute,
            _ => throw new NotSupportedException($"The specified truncation is not supported: {truncation}"),
        };
    }
}
