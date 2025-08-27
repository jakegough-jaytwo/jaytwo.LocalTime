using System;

namespace jaytwo.LocalTime;

public class LocalTimeTranslator : ILocalTimeTranslator
{
    internal const bool DefaultThrowOnInvalidLocalTime = LocalTimeService.DefaultThrowOnInvalidLocalTime;

    public LocalTimeTranslator(string inputTimeZoneId, string outputTimeZoneId, bool throwOnInvalidLocalTime = DefaultThrowOnInvalidLocalTime)
        : this(
            inputLocalTimeService: new LocalTimeService(inputTimeZoneId, throwOnInvalidLocalTime),
            outputLocalTimeService: new LocalTimeService(outputTimeZoneId, throwOnInvalidLocalTime))
    {
    }

    public LocalTimeTranslator(ILocalTimeService inputLocalTimeService, ILocalTimeService outputLocalTimeService)
    {
        InputLocalTimeService = inputLocalTimeService;
        OutputLocalTimeService = outputLocalTimeService;
    }

    public ILocalTimeService InputLocalTimeService { get; }

    public ILocalTimeService OutputLocalTimeService { get; }

    public string InputTimeZoneId => InputLocalTimeService.TimeZoneId;

    public string OutputTimeZoneId => OutputLocalTimeService.TimeZoneId;

    public object HealthCheck()
    {
        return new
        {
            InputLocalTimeService = InputLocalTimeService.HealthCheck(),
            OutputLocalTimeService = OutputLocalTimeService.HealthCheck(),
        };
    }

    /// <summary>
    /// Converts a local wall-clock <paramref name="localInput"/> in <see cref="InputTimeZoneId"/>
    /// to the equivalent local <see cref="DateTimeOffset"/> in <see cref="OutputTimeZoneId"/>.
    /// </summary>
    public DateTimeOffset ToOutputDateTimeOffset(DateTime input)
        => OutputLocalTimeService.GetLocalDateTimeOffset(
            InputLocalTimeService.GetDateTimeOffset(input));

    /// <summary>
    /// Same as <see cref="ToOutputLocalDateTimeOffset(DateTime)"/>, returning the local wall-clock
    /// <see cref="DateTime"/> (Kind=Unspecified) in <see cref="OutputTimeZoneId"/>.
    /// </summary>
    public DateTime ToOutputDateTime(DateTime input)
        => ToOutputDateTimeOffset(input).DateTime;
}
