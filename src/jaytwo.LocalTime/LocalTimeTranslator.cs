using System;

namespace jaytwo.LocalTime;

public class LocalTimeTranslator : ILocalTimeTranslator
{
    internal const bool DefaultThrowOnInvalidLocalTime = LocalTimeService.DefaultThrowOnInvalidInputTime;

    public LocalTimeTranslator(string inputTimeZoneId, string outputTimeZoneId, bool throwOnInvalidInputTime = DefaultThrowOnInvalidLocalTime)
        : this(
            inputLocalTimeService: new LocalTimeService(inputTimeZoneId, throwOnInvalidInputTime),
            outputLocalTimeService: new LocalTimeService(outputTimeZoneId, throwOnInvalidInputTime))
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
    /// Converts a local wall-clock <paramref name="localInput"/> in <see cref="InputTimeZoneId"/>
    /// to the equivalent local <see cref="DateTimeOffset"/> in <see cref="OutputTimeZoneId"/>.
    /// </summary>
    public DateTimeOffset ToOutputDateTimeOffset(DateTime input, bool throwOnInvalidInputTime)
        => OutputLocalTimeService.GetLocalDateTimeOffset(
            InputLocalTimeService.GetDateTimeOffset(input, throwOnInvalidInputTime));

    /// <summary>
    /// Same as <see cref="ToOutputLocalDateTimeOffset(DateTime)"/>, returning the local wall-clock
    /// <see cref="DateTime"/> (Kind=Unspecified) in <see cref="OutputTimeZoneId"/>.
    /// </summary>
    public DateTime ToOutputDateTime(DateTime input)
        => ToOutputDateTimeOffset(input).DateTime;

    /// <summary>
    /// Same as <see cref="ToOutputLocalDateTimeOffset(DateTime)"/>, returning the local wall-clock
    /// <see cref="DateTime"/> (Kind=Unspecified) in <see cref="OutputTimeZoneId"/>.
    /// </summary>
    public DateTime ToOutputDateTime(DateTime input, bool throwOnInvalidInputTime)
        => ToOutputDateTimeOffset(input, throwOnInvalidInputTime).DateTime;
}
