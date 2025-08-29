using System;
using Xunit;

namespace jaytwo.LocalTime.Tests;

public class LocalTimeTranslatorTests
{
    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", "America/Los_Angeles", "2025-01-01T04:34:56.789")]
    [InlineData("America/Denver", "2025-07-01T06:34:56.789", "America/Los_Angeles", "2025-07-01T05:34:56.789")]
    public void ToOutputDateTime_ReturnsExpected(string inputZone, string inputString, string ouputZone, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator(inputZone, ouputZone);
        var input = DateTime.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTime(input);

        // assert
        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00")]
    [InlineData("2025-03-09T02:30:00")]
    public void ToOutputDateTime_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles", throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.ToOutputDateTime(input));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00")]
    [InlineData("2025-03-09T02:30:00")]
    public void ToOutputDateTime_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles");
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.ToOutputDateTime(input, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T00:30:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T01:30:00")]
    public void ToOutputDateTime_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles", throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTime(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T00:30:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T01:30:00")]
    public void ToOutputDateTime_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles");
        var input = DateTime.Parse(inputString);
        var expected = DateTime.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTime(input, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("America/Denver", "2025-01-01T05:34:56.789", "America/Los_Angeles", "2025-01-01T04:34:56.789-08:00")]
    [InlineData("America/Denver", "2025-07-01T06:34:56.789", "America/Los_Angeles", "2025-07-01T05:34:56.789-07:00")]
    public void ToOutputDateTimeOffset_ReturnsExpected(string inputZone, string inputString, string ouputZone, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator(inputZone, ouputZone);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00")]
    [InlineData("2025-03-09T02:30:00")]
    public void ToOutputDateTimeOffset_Throws_with_class_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles", throwOnAmbiguousOrSkipped: true);
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.ToOutputDateTimeOffset(input));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00")]
    [InlineData("2025-03-09T02:30:00")]
    public void ToOutputDateTimeOffset_Throws_with_method_throwOnAmbiguousOrSkipped_true(string inputString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles");
        var input = DateTime.Parse(inputString);

        // act & assert
        var exception = Assert.ThrowsAny<Exception>(() => sut.ToOutputDateTimeOffset(input, throwOnAmbiguousOrSkipped: true));
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T07:30:00+00:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T09:30:00+00:00")]
    public void ToOutputDateTimeOffset_resolves_with_class_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles", throwOnAmbiguousOrSkipped: false);
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTimeOffset(input);

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2025-11-02T01:30:00", "2025-11-02T07:30:00+00:00")]
    [InlineData("2025-03-09T02:30:00", "2025-03-09T09:30:00+00:00")]
    public void ToOutputDateTimeOffset_resolves_with_method_throwOnAmbiguousOrSkipped_false(string inputString, string expectedString)
    {
        // arragne
        var sut = new LocalTimeTranslator("America/Denver", "America/Los_Angeles");
        var input = DateTime.Parse(inputString);
        var expected = DateTimeOffset.Parse(expectedString);

        // act
        var actual = sut.ToOutputDateTimeOffset(input, throwOnAmbiguousOrSkipped: false);

        // assert
        Assert.Equal(expected, actual);
    }
}
