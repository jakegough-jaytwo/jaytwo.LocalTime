using System;

namespace jaytwo.LocalTime;

public interface ILocalTimeService
{
    string TimeZoneId { get; }

    DateTimeOffset UtcNow { get; }

    DateTimeOffset LocalNow { get; }

    DateTime GetUtcDateTimeFromLocal(DateTime input);

    DateTime GetUtcDateTimeFromLocal(DateTime input, bool throwOnInvalidLocalTime = false);

    DateTimeOffset GetLocalDateTimeOffset(DateTime input);

    DateTimeOffset GetLocalDateTimeOffset(DateTime input, bool throwOnInvalidLocalTime = false);

    DateTimeOffset GetLocalDateTimeOffset(DateTimeOffset input);

    DateTimeOffset GetLocalDateTimeOffsetFromUnixTimeSeconds(long input);

    DateTimeOffset GetLocalDateTimeOffsetFromUnixTimeMilliseconds(long input);

    DateTime GetLocalDateTime(DateTimeOffset input);

    DateTime GetLocalDateTimeFromUtc(DateTime input);

    DateTime GetLocalDateTimeFromUnixTimeSeconds(long input);

    DateTime GetLocalDateTimeFromUnixTimeMilliseconds(long input);
}
