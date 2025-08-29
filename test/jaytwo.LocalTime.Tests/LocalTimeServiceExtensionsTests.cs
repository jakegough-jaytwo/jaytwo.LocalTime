using System;
using System.Runtime.CompilerServices;
using NodaTime;
using Xunit;

namespace jaytwo.LocalTime.Tests;

public class LocalTimeServiceExtensionsTests
{
    [Theory]
    [InlineData("America/Denver", "2025-03-09T00:00:00", 1, "2025-03-09T01:00:00-07:00")]
    [InlineData("America/Denver", "2025-03-09T01:00:00", 1, "2025-03-09T03:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:00:00", 1, "2025-11-02T01:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:00:00", 2, "2025-11-02T01:00:00-07:00")]
    public void AddHours_Returns_Expected(string zone, string inputString, double addHours, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddHours(input, addHours);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddHours_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddHours(input, 0, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddHours_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddHours(input, 0));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddHours_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddHours(input, 0, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddHours_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddHours(input, 0);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-03-09T00:59:00", 1, "2025-03-09T01:00:00-07:00")]
    [InlineData("America/Denver", "2025-03-09T01:59:00", 1, "2025-03-09T03:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:59:00", 1, "2025-11-02T01:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:59:00", 61, "2025-11-02T01:00:00-07:00")]
    public void AddMinutes_Returns_Expected(string zone, string inputString, double addHours, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddMinutes(input, addHours);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddMinutes_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddMinutes(input, 0, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddMinutes_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddMinutes(input, 0));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddMinutes_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddMinutes(input, 0, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddMinutes_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddMinutes(input, 0);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-03-09T00:00:59", 1, "2025-03-09T00:01:00-07:00")]
    [InlineData("America/Denver", "2025-03-09T00:59:59", 1, "2025-03-09T01:00:00-07:00")]
    [InlineData("America/Denver", "2025-03-09T01:59:00", 1, "2025-03-09T01:59:01-07:00")]
    [InlineData("America/Denver", "2025-03-09T01:59:59", 1, "2025-03-09T03:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:00:59", 1, "2025-11-02T00:01:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:59:59", 1, "2025-11-02T01:00:00-06:00")]
    [InlineData("America/Denver", "2025-11-02T00:59:59", 3601, "2025-11-02T01:00:00-07:00")]
    public void AddSeconds_Returns_Expected(string zone, string inputString, double addHours, string expectedString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddSeconds(input, addHours);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddSeconds_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddSeconds(input, 0, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void AddSeconds_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.AddSeconds(input, 0));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddSeconds_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddSeconds(input, 0, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void AddSeconds_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.AddSeconds(input, 0);

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
        ILocalTimeService sut = new LocalTimeService(zone);
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
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeFromUtc(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56.789", "2025-01-01T05:34:56.789-07:00")]
    [InlineData("America/Denver", "2025-07-01T12:34:56.789", "2025-07-01T06:34:56.789-06:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T12:34:56.789", "2025-01-01T04:34:56.789-08:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T12:34:56.789", "2025-07-01T05:34:56.789-07:00")]
    public void GetLocalDateTimeOffsetFromUtc_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetLocalDateTimeOffsetFromUtc(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-07-01T06:34:56.789", "2025-07-01T12:34:56.789+00:00")]
    [InlineData("America/Los_Angeles", "2025-01-01T04:34:56.789", "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Los_Angeles", "2025-07-01T05:34:56.789", "2025-07-01T12:34:56.789+00:00")]
    public void GetUtcDateTime_ReturnsExpected(string zone, string inputString, string expectedString)
    {
        // arragne
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTime(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Unspecified, "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Utc, "2025-01-01T12:34:56.789+00:00")]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", DateTimeKind.Local, "2025-01-01T12:34:56.789+00:00")]
    public void GetUtcDateTime_ReturnsExpected_with_any_DateTimeKid(string zone, string inputString, DateTimeKind inputKind, string expectedString)
    {
        // arragne
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.SpecifyKind(DateTime.Parse(inputString), inputKind);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTime(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000")]
    [InlineData("2025-11-02T01:30:00.000")]
    public void GetUtcDateTime_throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetUtcDateTime(input));

        // assert
    }

    [Theory]
    [InlineData("2025-03-09T02:30:00.000")]
    [InlineData("2025-11-02T01:30:00.000")]
    public void GetUtcDateTime_throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetUtcDateTime(input, throwOnAmbiguousOrSkipped: true));

        // assert
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void GetUtcDateTime_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTime(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void GetUtcDateTime_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString).UtcDateTime;

        // act
        var actual = sut.GetUtcDateTime(input, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);
    }

    [Fact]
    public void GetUtcDateTimeOffset_Returns_Expected()
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse("2025-11-02T01:30:00.000");
        var expected = DateTimeOffset.Parse("2025-11-02T07:30:00.000+00:00");

        // act
        var actual = sut.GetUtcDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void GetUtcDateTimeOffset_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetUtcDateTimeOffset(input, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000")]
    [InlineData("2025-03-09T02:30:00.000")]
    public void GetUtcDateTimeOffset_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.GetUtcDateTimeOffset(input));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void GetUtcDateTimeOffset_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetUtcDateTimeOffset(input, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00.000", "2025-11-02T07:30:00.000+00:00")]
    [InlineData("2025-03-09T02:30:00.000", "2025-03-09T09:30:00.000+00:00")]
    public void GetUtcDateTimeOffset_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.GetUtcDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T12:34:56", "2025-01-01T12:34:55", "00:00:01")]
    [InlineData("America/Denver", "2025-03-09T03:00:00", "2025-03-09T00:00:00", "02:00:00")]
    [InlineData("America/Denver", "2025-11-02T03:00:00", "2025-11-02T00:00:00", "04:00:00")]
    public void Subtract_Returns_Expected(string zone, string baseTimeString, string subtractTimeString, string expectedDifferenceString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var baseTime = DateTime.Parse(baseTimeString);
        var subtractTime = DateTime.Parse(subtractTimeString);
        var expectedDifference = TimeSpan.Parse(expectedDifferenceString);

        // act
        var actual = sut.Subtract(baseTime, subtractTime);

        // assert
        Assert.Equal(expectedDifference, actual);
    }

    [Theory]
    [InlineData("2025-11-02T03:00:00", "2025-11-02T01:30:00")]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T00:00:00")]
    [InlineData("2025-03-09T03:00:00", "2025-03-09T02:30:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T00:00:00")]
    public void Subtract_Throws_with_method_throwOnAmbiguousOrSkipped_true(string baseTimeString, string subtractTimeString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone);
        var baseTime = DateTime.Parse(baseTimeString);
        var subtractTime = DateTime.Parse(subtractTimeString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.Subtract(baseTime, subtractTime, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T03:00:00", "2025-11-02T01:30:00")]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T00:00:00")]
    [InlineData("2025-03-09T03:00:00", "2025-03-09T02:30:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T00:00:00")]
    public void Subtract_Throws_with_class_throwOnAmbiguousOrSkipped_true(string baseTimeString, string subtractTimeString)
    {
        // arragne
        var zone = "America/Denver";
        ILocalTimeService sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: true);
        var baseTime = DateTime.Parse(baseTimeString);
        var subtractTime = DateTime.Parse(subtractTimeString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.Subtract(baseTime, subtractTime));
    }

    [Theory]
    [InlineData("America/Denver", "2025-03-09T02:30:00", "2025-03-09T00:00:00", "02:30:00")]
    [InlineData("America/Denver", "2025-03-09T03:00:00", "2025-03-09T02:30:00", "-00:30:00")]
    [InlineData("America/Denver", "2025-11-02T01:30:00", "2025-11-02T00:00:00", "01:30:00")]
    [InlineData("America/Denver", "2025-11-02T03:00:00", "2025-11-02T01:30:00", "02:30:00")]
    public void Subtract_resolves_with_class_throwOnAmbiguousOrSkipped_false(string zone, string baseTimeString, string subtractTimeString, string expectedDifferenceString)
    {
        // arragne
        var sut = new LocalTimeService(zone, throwOnAmbiguousOrSkipped: false);
        var baseTime = DateTime.Parse(baseTimeString);
        var subtractTime = DateTime.Parse(subtractTimeString);
        var expectedDifference = TimeSpan.Parse(expectedDifferenceString);

        // act
        var actual = sut.Subtract(baseTime, subtractTime);

        // assert
        Assert.Equal(expectedDifference, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-03-09T02:30:00", "2025-03-09T00:00:00", "02:30:00")]
    [InlineData("America/Denver", "2025-03-09T03:00:00", "2025-03-09T02:30:00", "-00:30:00")]
    [InlineData("America/Denver", "2025-11-02T01:30:00", "2025-11-02T00:00:00", "01:30:00")]
    [InlineData("America/Denver", "2025-11-02T03:00:00", "2025-11-02T01:30:00", "02:30:00")]
    public void Subtract_resolves_with_method_throwOnAmbiguousOrSkipped_false(string zone, string baseTimeString, string subtractTimeString, string expectedDifferenceString)
    {
        // arragne
        var sut = new LocalTimeService(zone);
        var baseTime = DateTime.Parse(baseTimeString);
        var subtractTime = DateTime.Parse(subtractTimeString);
        var expectedDifference = TimeSpan.Parse(expectedDifferenceString);

        // act
        var actual = sut.Subtract(baseTime, subtractTime, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expectedDifference, actual);
    }
}
