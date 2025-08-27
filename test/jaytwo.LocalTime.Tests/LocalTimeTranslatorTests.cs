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
}
