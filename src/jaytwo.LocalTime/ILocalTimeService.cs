using System;

namespace jaytwo.LocalTime;

public interface ILocalTimeService
{
    string TimeZoneId { get; }

    DateTimeOffset UtcNow { get; }

    DateTimeOffset LocalNow { get; }

    object HealthCheck();

    ResolveTimeResult Resolve(DateTime local);

#if NET6_0_OR_GREATER
    DateTimeOffset GetDateTimeOffset(DateOnly localDate, TimeOnly localTime);
#endif

    DateTimeOffset GetDateTimeOffset(DateTime local);

    DateTimeOffset GetDateTimeOffset(DateTime local, bool throwOnAmbiguousOrSkipped);

    DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input);

    DateTimeOffset GetStartOfWeek(DateTime local);

#if NET6_0_OR_GREATER
    DateTimeOffset GetStartOfWeek(DateOnly localDate);
#endif

    DateTimeOffset GetStartOfWeek(DateTimeOffset input);
}
