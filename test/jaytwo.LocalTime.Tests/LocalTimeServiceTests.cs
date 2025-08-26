using System;
using NodaTime;
using Xunit;

namespace jaytwo.LocalTime.Tests;

public class LocalTimeServiceTests
{
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

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T05:34:56.000-07:00")]
    public void LocalNow_return_expected(string zone, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone) { UtcNowFactory = () => input };
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
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T12:34:56.000+00:00")]
    public void UtcNow_return_expected(string zone, string inputString, string expectedString)
    {
        // arrange
        var input = DateTimeOffset.Parse(inputString);
        var sut = new LocalTimeService(zone) { UtcNowFactory = () => input };
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.UtcNow;

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

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T05:34:56.000-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.000+00:00", "2025-07-01T06:34:56.000-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.000+00:00", "2025-01-01T04:34:56.000-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.000+00:00", "2025-07-01T05:34:56.000-07:00")]
    public void GetLocalDateTimeOffsetFromUnixTimeSeconds_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString).ToUnixTimeSeconds();
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeOffsetFromUnixTimeSeconds(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789+00:00", "2025-01-01T05:34:56.789-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789+00:00", "2025-07-01T06:34:56.789-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789+00:00", "2025-01-01T04:34:56.789-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789+00:00", "2025-07-01T05:34:56.789-07:00")]
    public void GetLocalDateTimeOffsetFromUnixTimeMilliseconds_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString).ToUnixTimeMilliseconds();
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeOffsetFromUnixTimeMilliseconds(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789+00:00", "2025-01-01T05:34:56.789")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789+00:00", "2025-07-01T06:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789+00:00", "2025-01-01T04:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789+00:00", "2025-07-01T05:34:56.789")]
    public void GetLocalDateTime_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTime(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789", "2025-01-01T05:34:56.789")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789", "2025-07-01T06:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789", "2025-01-01T04:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789", "2025-07-01T05:34:56.789")]
    public void GetLocalDateTimeFromUtc_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeFromUtc(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.000+00:00", "2025-01-01T05:34:56.000")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.000+00:00", "2025-07-01T06:34:56.000")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.000+00:00", "2025-01-01T04:34:56.000")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.000+00:00", "2025-07-01T05:34:56.000")]
    public void GetLocalDateTimeFromUnixTimeSeconds_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString).ToUnixTimeSeconds();
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeFromUnixTimeSeconds(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789+00:00", "2025-01-01T05:34:56.789")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789+00:00", "2025-07-01T06:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789+00:00", "2025-01-01T04:34:56.789")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789+00:00", "2025-07-01T05:34:56.789")]
    public void GetLocalDateTimeFromUnixTimeMilliseconds_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTimeOffset.Parse(inputString).ToUnixTimeMilliseconds();
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeFromUnixTimeMilliseconds(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-07-01T06:34:56.789", "2025-07-01T12:34:56.789+00:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T04:34:56.789", "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T05:34:56.789", "2025-07-01T12:34:56.789+00:00")]
    public void GetUtcDateTimeFromLocal_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Unspecified, "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Utc, "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Local, "2025-01-01T12:34:56.789+00:00")]
    public void GetUtcDateTimeFromLocal_ReturnsExpected_with_any_DateTimeKid(string zone, string inputString, DateTimeKind inputKind, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.SpecifyKind(DateTime.Parse(inputString), inputKind);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_throws_SkippedTimeException_with_class_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = true };
        var input = DateTime.Parse("2025-03-09T02:30:00.000");

        // act
        var exception = Assert.Throws<SkippedTimeException>(() => sut.GetUtcDateTimeFromLocal(input));

        // assert
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_throws_SkippedTimeException_with_method_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-03-09T02:30:00.000");

        // act
        var exception = Assert.Throws<SkippedTimeException>(() => sut.GetUtcDateTimeFromLocal(input, throwOnInvalidLocalTime: true));

        // assert
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_resolves_skipped_with_class_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = false };
        var input = DateTime.Parse("2025-03-09T02:30:00.000");
        var expected = DateTimeOffset.Parse("2025-03-09T09:30:00.000+00:00").UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_resolves_skipped_with_method_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-03-09T02:30:00.000");
        var expected = DateTimeOffset.Parse("2025-03-09T09:30:00.000+00:00").UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input, throwOnInvalidLocalTime: false);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_throws_AmbiguousTimeException_with_class_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = true };
        var input = DateTime.Parse("2025-11-02T01:30:00.000");

        // act
        var exception = Assert.Throws<AmbiguousTimeException>(() => sut.GetUtcDateTimeFromLocal(input));

        // assert
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_throws_AmbiguousTimeException_with_method_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-11-02T01:30:00.000");

        // act
        var exception = Assert.Throws<AmbiguousTimeException>(() => sut.GetUtcDateTimeFromLocal(input, throwOnInvalidLocalTime: true));

        // assert
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_resolves_ambiguous_with_class_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = false };
        var input = DateTime.Parse("2025-11-02T01:30:00.000");
        var expected = DateTimeOffset.Parse("2025-11-02T07:30:00.000+00:00").UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void GetUtcDateTimeFromLocal_resolves_ambiguous_with_method_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-11-02T01:30:00.000");
        var expected = DateTimeOffset.Parse("2025-11-02T07:30:00.000+00:00").UtcDateTime;

        // act
        var actual = sut.GetUtcDateTimeFromLocal(input, throwOnInvalidLocalTime: false);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789", "2025-01-01T12:34:56.789-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789", "2025-07-01T12:34:56.789-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789", "2025-01-01T12:34:56.789-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789", "2025-07-01T12:34:56.789-07:00")]
    public void GetLocalDateTimeOffset_with_DateTime_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetLocalDateTimeOffset_throws_SkippedTimeException_with_class_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = true };
        var input = DateTime.Parse("2025-03-09T02:30:00.000");

        // act
        var exception = Assert.Throws<SkippedTimeException>(() => sut.GetLocalDateTimeOffset(input));

        // assert
    }

    [Fact]
    public void GetLocalDateTimeOffset_throws_SkippedTimeException_with_method_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-03-09T02:30:00.000");

        // act
        var exception = Assert.Throws<SkippedTimeException>(() => sut.GetLocalDateTimeOffset(input, throwOnInvalidLocalTime: true));

        // assert
    }

    [Fact]
    public void GetLocalDateTimeOffset_resolves_skipped_with_class_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = false };
        var input = DateTime.Parse("2025-03-09T02:30:00.000");
        var expected = DateTimeOffset.Parse("2025-03-09T02:30:00.000-07:00");

        // act
        var actual = sut.GetLocalDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetLocalDateTimeOffset_resolves_skipped_with_method_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-03-09T02:30:00.000");
        var expected = DateTimeOffset.Parse("2025-03-09T03:30:00.000-06:00");

        // act
        var actual = sut.GetLocalDateTimeOffset(input, throwOnInvalidLocalTime: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetLocalDateTimeOffset_throws_AmbiguousTimeException_with_class_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = true };
        var input = DateTime.Parse("2025-11-02T01:30:00.000");

        // act
        var exception = Assert.Throws<AmbiguousTimeException>(() => sut.GetLocalDateTimeOffset(input));

        // assert
    }

    [Fact]
    public void GetLocalDateTimeOffset_throws_AmbiguousTimeException_with_method_throwOnInvalidLocalTime_true()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-11-02T01:30:00.000");

        // act
        var exception = Assert.Throws<AmbiguousTimeException>(() => sut.GetLocalDateTimeOffset(input, throwOnInvalidLocalTime: true));

        // assert
    }

    [Fact]
    public void GetLocalDateTimeOffset_resolves_ambiguous_with_class_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone) { ThrowOnInvalidLocalTime = false };
        var input = DateTime.Parse("2025-11-02T01:30:00.000");
        var expected = DateTimeOffset.Parse("2025-11-02T01:30:00.000-06:00");

        // act
        var actual = sut.GetLocalDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetLocalDateTimeOffset_resolves_ambiguous_with_method_throwOnInvalidLocalTime_false()
    {
        // arragne
        var zone = "America/Denver";
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-11-02T01:30:00.000");
        var expected = DateTimeOffset.Parse("2025-11-02T01:30:00.000-06:00");

        // act
        var actual = sut.GetLocalDateTimeOffset(input, throwOnInvalidLocalTime: false);

        // assert
        Assert.Equal(expected, actual);
    }
}
