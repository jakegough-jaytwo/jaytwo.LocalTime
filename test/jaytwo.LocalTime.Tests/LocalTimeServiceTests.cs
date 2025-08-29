using System;
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
    [InlineData("2025-03-09T02:30:00.000")]
    [InlineData("2025-11-02T01:30:00.000")]
    public void GetDateTimeOffset_throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetDateTimeOffset(input));
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
        var sut = LocalTimeService.Create(zone, utcNowFactory: () => input);
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
        var sut = LocalTimeService.Create(zone, utcNowFactory: () => input);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.UtcNow;

        // assert
        Assert.Equal(expected, actual);
    }
}
