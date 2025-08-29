using System;

namespace jaytwo.LocalTime;

public interface ILocalTimeService
{
    string TimeZoneId { get; }

    DateTimeOffset UtcNow { get; }

    DateTimeOffset LocalNow { get; }

    object HealthCheck();

    DateTimeOffset GetDateTimeOffset(DateTime local);

    DateTimeOffset GetDateTimeOffset(DateTime local, bool throwOnAmbiguousOrSkipped);

    DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input);
}
