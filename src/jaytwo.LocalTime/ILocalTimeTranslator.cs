using System;

namespace jaytwo.LocalTime;

public interface ILocalTimeTranslator
{
    string InputTimeZoneId { get; }

    string OutputTimeZoneId { get; }

    object HealthCheck();

    DateTimeOffset ToOutputDateTimeOffset(DateTime input);

    DateTimeOffset ToOutputDateTimeOffset(DateTime input, bool throwOnInvalidInputTime);

    DateTime ToOutputDateTime(DateTime input);

    DateTime ToOutputDateTime(DateTime input, bool throwOnInvalidInputTime);
}
