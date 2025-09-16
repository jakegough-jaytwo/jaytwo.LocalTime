using System;

namespace jaytwo.LocalTime;

public class ResolveTimeResult
{
    public DateTimeOffset? ForwardShifted { get; set; }

    public DateTimeOffset? StartOfIntervalAfter { get; set; }

    public DateTimeOffset[] Matches { get; set; } = Array.Empty<DateTimeOffset>();

    public bool IsAmbiguous() => Matches.Length == 2;

    public bool IsSkipped() => Matches.Length == 0;
}
