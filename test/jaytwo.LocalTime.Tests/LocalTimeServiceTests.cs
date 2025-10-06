using System;
using System.Linq;
using NodaTime;
using Xunit;

namespace jaytwo.LocalTime.Tests;

public class LocalTimeServiceTests
{
    [Fact]
    public void Ctor_InvalidZone_ThrowsArgumentException()
    {
        // arragne
        var zone = "Not/AZone";

        // act
        var ex = Assert.Throws<ArgumentException>(() => new LocalTimeService(zone));

        // assert
        Assert.Equal("timeZoneId", ex.ParamName);
        Assert.Contains("Could not resolve time zone", ex.Message);
    }

    [Fact]
    public void Ctor_ValidZone_SetsTimeZoneId()
    {
        // arragne
        var zone = "America/Denver";

        // act
        var sut = new LocalTimeService(zone);

        // assert
        Assert.Equal("America/Denver", sut.TimeZoneId);
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T03:30:00.000-06:00")]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T01:30:00.000-06:00")]
    public void GetDateTimeOffset_resolves_time_leniently_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T03:30:00.000-06:00")]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T01:30:00.000-06:00")]
    public void GetDateTimeOffset_resolves_time_leniently_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetDateTimeOffset(input, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    public void GetDateTimeOffset_throws_AmbiguousTimeException_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.Throws<AmbiguousTimeException>(() => sut.GetDateTimeOffset(input));
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000")]
    public void GetDateTimeOffset_throws_SkippedTimeException_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.Throws<SkippedTimeException>(() => sut.GetDateTimeOffset(input));
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000")]
    [InlineData("2025-11-02T01:30:00.000")]
    public void GetDateTimeOffset_throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetDateTimeOffset(input, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789", "2025-01-01T12:34:56.789-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789", "2025-07-01T12:34:56.789-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789", "2025-01-01T12:34:56.789-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789", "2025-07-01T12:34:56.789-07:00")]
    public void GetDateTimeOffset_with_DateTime_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(DateTimeKind.Unspecified)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    public void GetDateTimeOffset_works_with_any_DateTimeKind(DateTimeKind kind)
    {
        // arrange
        var timeZoneId = "America/Denver";
        var timeZoneCronSchedule = new LocalTimeService(timeZoneId);
        var expected = DateTimeOffset.Parse("1970-01-01 01:01:01.001-07:00");
        var inputDateTime = new DateTime(1970, 01, 01, 01, 01, 01, 01, kind);

        // act
        var actual = timeZoneCronSchedule.GetDateTimeOffset(inputDateTime);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789+00:00", "2025-01-01T05:34:56.789-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789+00:00", "2025-07-01T06:34:56.789-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789+00:00", "2025-01-01T04:34:56.789-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789+00:00", "2025-07-01T05:34:56.789-07:00")]
    public void GetLocalDateTimeOffset_with_DateTimeOffset_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LocalNow_default_factory_is_close_to_actual_now()
    {
        // arrange
        var sut = new LocalTimeService("UTC");
        var expected = DateTimeOffset.UtcNow;

        // act
        var actual = sut.LocalNow;

        // assert
        Assert.Equal(expected.DateTime, actual.DateTime, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T05:34:56.000-07:00")]
    public void LocalNow_returns_expected(string zone, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone, utcNowFactory: () => input);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.LocalNow;

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UtcNow_default_factory_is_close_to_actual_now()
    {
        // arrange
        var sut = new LocalTimeService("America/Denver");
        var expected = DateTimeOffset.UtcNow;

        // act
        var actual = sut.UtcNow;

        // assert
        Assert.Equal(expected.DateTime, actual.DateTime, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T12:34:56.000+00:00")]
    public void UtcNow_return_expected(string zone, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone, utcNowFactory: () => input);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.UtcNow;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", TimePrecision.None, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T12:34:56.12345678+00:00")]
    [InlineData("America/Denver", TimePrecision.Microsecond, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T12:34:56.123456+00:00")]
    [InlineData("America/Denver", TimePrecision.Millisecond, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T12:34:56.123+00:00")]
    [InlineData("America/Denver", TimePrecision.Second, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T12:34:56.000+00:00")]
    [InlineData("America/Denver", TimePrecision.Minute, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T12:34:00.000+00:00")]
    public void UtcNow_WithPrevision_return_expected(string zone, TimePrecision precision, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone, nowPrecision: precision, utcNowFactory: () => input);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.UtcNow;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", TimePrecision.None, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T05:34:56.12345678-07:00")]
    [InlineData("America/Denver", TimePrecision.Microsecond, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T05:34:56.123456-07:00")]
    [InlineData("America/Denver", TimePrecision.Millisecond, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T05:34:56.123-07:00")]
    [InlineData("America/Denver", TimePrecision.Second, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T05:34:56.000-07:00")]
    [InlineData("America/Denver", TimePrecision.Minute, "2025-01-01T12:34:56.12345678+00:00", "2025-01-01T05:34:00.000-07:00")]
    public void LocalNow_WithPrevision_return_expected(string zone, TimePrecision precision, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone, nowPrecision: precision, utcNowFactory: () => input);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.LocalNow;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-11-02T00:59:00")]
    [InlineData("America/Denver", "2025-11-02T02:00:00")]
    [InlineData("America/Denver", "2025-03-09T01:59:00")]
    [InlineData("America/Denver", "2025-03-09T03:00:00")]
    public void Resolve_Unambiguous_Times(string zone, string inputString)
    {
        // arrange
        var input = DateTime.Parse(inputString);
        var sut = new LocalTimeService(zone);
        var expected = sut.GetDateTimeOffset(input, throwOnAmbiguousOrSkipped: true);

        // act
        var actual = sut.Resolve(input);

        // assert
        Assert.False(actual.IsSkipped());
        Assert.False(actual.IsAmbiguous());
        var match = Assert.Single(actual.Matches);
        Assert.Equal(expected, match);
        Assert.Null(actual.ForwardShifted);
        Assert.Null(actual.StartOfIntervalAfter);
    }

    [Theory]
    [InlineData("America/Denver", "2025-03-09T02:00:00", "2025-03-09T03:00:00-06:00", "2025-03-09T03:00:00-06:00")]
    [InlineData("America/Denver", "2025-03-09T02:59:59", "2025-03-09T03:00:00-06:00", "2025-03-09T03:59:59-06:00")]
    public void Resolve_Skipped_Times(string zone, string inputString, string expectedStartOfIntervalAfterString, string expectedForwardShiftedString)
    {
        // arrange
        var input = DateTime.Parse(inputString);
        var sut = new LocalTimeService(zone);
        var expectedForwardShifted = DateTimeOffset.Parse(expectedForwardShiftedString);
        var expectedStartOfIntervalAfter = DateTimeOffset.Parse(expectedStartOfIntervalAfterString);

        // act
        var actual = sut.Resolve(input);

        // assert
        Assert.True(actual.IsSkipped());
        Assert.False(actual.IsAmbiguous());
        Assert.Empty(actual.Matches);
        Assert.Equal(expectedForwardShifted, actual.ForwardShifted);
        Assert.Equal(expectedStartOfIntervalAfter, actual.StartOfIntervalAfter);
    }

    [Theory]
    [InlineData("America/Denver", "2025-11-02T01:00:00", "2025-11-02T01:00:00-06:00", "2025-11-02T01:00:00-07:00")]
    [InlineData("America/Denver", "2025-11-02T01:59:59", "2025-11-02T01:59:59-06:00", "2025-11-02T01:59:59-07:00")]
    public void Resolve_Ambiguous_Times(string zone, string inputString, string expectedFirstMatchString, string expectedlastMatchString)
    {
        // arrange
        var input = DateTime.Parse(inputString);
        var sut = new LocalTimeService(zone);
        var expectedFirstMatch = DateTimeOffset.Parse(expectedFirstMatchString);
        var expectedlastMatch = DateTimeOffset.Parse(expectedlastMatchString);

        // act
        var actual = sut.Resolve(input);

        // assert
        Assert.True(actual.IsAmbiguous());
        Assert.False(actual.IsSkipped());
        Assert.Equal(2, actual.Matches.Length);
        Assert.Equal(expectedFirstMatch, actual.Matches.First());
        Assert.Equal(expectedlastMatch, actual.Matches.Last());
        Assert.Null(actual.ForwardShifted);
        Assert.Null(actual.StartOfIntervalAfter);
    }

    [Theory]
    [InlineData("America/Denver", "2023-01-12 01:23:45", DayOfWeek.Sunday, "2023-01-08T00:00:00-07:00")]
    [InlineData("America/Denver", "2023-01-12 01:23:45", DayOfWeek.Monday, "2023-01-09T00:00:00-07:00")]
    [InlineData("America/Denver", "2023-07-12 01:23:45", DayOfWeek.Sunday, "2023-07-09T00:00:00-06:00")]
    [InlineData("America/Denver", "2023-07-12 01:23:45", DayOfWeek.Monday, "2023-07-10T00:00:00-06:00")]
    public void GetStartOfWeek_DateTime_Returns_Expected(string zone, string inputStr, DayOfWeek firstDayOfWeek, string expectedStr)
    {
        // arrange
        var input = DateTime.Parse(inputStr);
        var sut = new LocalTimeService(zone, firstDayOfWeek: firstDayOfWeek);
        var expected = DateTime.Parse(expectedStr);

        // act
        var actual = sut.GetStartOfWeek(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2023-01-12 01:23:45-07:00", DayOfWeek.Sunday, "2023-01-08T00:00:00-07:00")]
    [InlineData("America/Denver", "2023-01-12 01:23:45-07:00", DayOfWeek.Monday, "2023-01-09T00:00:00-07:00")]
    [InlineData("America/Denver", "2023-07-12 01:23:45-06:00", DayOfWeek.Sunday, "2023-07-09T00:00:00-06:00")]
    [InlineData("America/Denver", "2023-07-12 01:23:45-06:00", DayOfWeek.Monday, "2023-07-10T00:00:00-06:00")]
    [InlineData("America/Denver", "2023-01-07 23:12:34-12:00", DayOfWeek.Sunday, "2023-01-08T00:00:00-07:00")] // crossing the international date line
    public void GetStartOfWeek_DateTimeOffset_Returns_Expected(string zone, string inputStr, DayOfWeek firstDayOfWeek, string expectedStr)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputStr);
        var sut = new LocalTimeService(zone, firstDayOfWeek: firstDayOfWeek);
        var expected = DateTimeOffset.Parse(expectedStr);

        // act
        var actual = sut.GetStartOfWeek(input);

        // assert
        Assert.Equal(expected, actual);
    }
}
