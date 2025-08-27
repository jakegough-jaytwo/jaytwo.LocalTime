using System;
using System.Globalization;
using Xunit;

namespace jaytwo.LocalTime.Tests;

public class OtherTests
{
    [Theory]
    [InlineData("2025-01-01T12:34:56")]
    [InlineData("2025-01-01T12:34:56.789")]
    public void DateTime_Parse_Quirky_DateTimeKind_Unspecified(string inputString)
    {
        // arragne
        var inputAsOffset = DateTimeOffset.Parse(inputString);

        // act
        var actual = DateTime.Parse(inputString);

        // assert
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
        Assert.Equal(inputAsOffset.DateTime, actual);
    }

    [Theory]
    [InlineData("2025-01-01T12:34:56Z")]
    [InlineData("2025-01-01T12:34:56.789Z")]
    [InlineData("2025-01-01T12:34:56+00:00")]
    [InlineData("2025-01-01T12:34:56.789+00:00")]
    public void DateTime_Parse_Quirky_DateTimeKind_Local(string inputString)
    {
        // arragne
        var inputAsOffset = DateTimeOffset.Parse(inputString);

        // act
        var actual = DateTime.Parse(inputString);

        // assert
        Assert.Equal(DateTimeKind.Local, actual.Kind);
        Assert.Equal(inputAsOffset.LocalDateTime, actual); // always Kind = Local
    }

    [Fact]
    public void DateTimeKind_Local_ToString()
    {
        // arragne
        var inputString = "2025-01-01T12:34:56.7891234";
        var inputKind = DateTimeKind.Local;
        var input = DateTime.SpecifyKind(DateTime.Parse(inputString), inputKind);
        var inputAsOffset = DateTimeOffset.Parse(inputString);
        var expected = inputAsOffset.ToString("o", CultureInfo.InvariantCulture);

        // act
        var actual = input.ToString("o", CultureInfo.InvariantCulture);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DateTimeKind_Utc_ToString()
    {
        // arragne
        var inputString = "2025-01-01T12:34:56.7891234";
        var inputKind = DateTimeKind.Utc;
        var input = DateTime.SpecifyKind(DateTime.Parse(inputString), inputKind);
        var inputAsOffset = DateTimeOffset.Parse(inputString);
        var expected = inputString + "Z";

        // act
        var actual = input.ToString("o", CultureInfo.InvariantCulture);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DateTimeKind_Unspecified_ToString()
    {
        // arragne
        var inputString = "2025-01-01T12:34:56.7891234";
        var inputKind = DateTimeKind.Unspecified;
        var input = DateTime.SpecifyKind(DateTime.Parse(inputString), inputKind);
        var inputAsOffset = DateTimeOffset.Parse(inputString);
        var expected = inputString;

        // act
        var actual = input.ToString("o", CultureInfo.InvariantCulture);

        // assert
        Assert.Equal(expected, actual);
    }
}
