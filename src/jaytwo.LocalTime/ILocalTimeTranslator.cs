using System;

namespace jaytwo.LocalTime;

public interface ILocalTimeTranslator
{
    string InputTimeZoneId { get; }

    string OutputTimeZoneId { get; }

    object HealthCheck();

    DateTimeOffset ToOutputDateTimeOffset(DateTime input);

    DateTime ToOutputDateTime(DateTime input);
}
