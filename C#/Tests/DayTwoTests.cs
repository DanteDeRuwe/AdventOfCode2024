using FluentAssertions;

namespace Tests;

public class DayTwoTests
{
    [Fact]
    public void Test1()
    {
        var lines = """
                    7 6 4 2 1
                    1 2 7 8 9
                    9 7 6 2 1
                    1 3 2 4 5
                    8 6 4 4 1
                    1 3 6 7 9
                    """.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        DayTwo.PartOne(lines).Should().Be(2);
    }

    [Fact]
    public void Test2()
    {
        var lines = """
                    7 6 4 2 1
                    1 2 7 8 9
                    9 7 6 2 1
                    1 3 2 4 5
                    8 6 4 4 1
                    1 3 6 7 9
                    """.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        DayTwo.PartTwo(lines).Should().Be(4);
    }

    [Theory]
    [InlineData("1 2 3 4 5", true)]
    [InlineData("5 4 3 2 1", true)]
    [InlineData("1 4 7 10 13", true)]
    [InlineData("20 10 8 6 4 2", true, "Safe when remove 20")]
    [InlineData("1 2 3 40 7 6", false, "Unsafe: even if we remove 40; 3 to 7 is still to high")]
    public void Test2_Custom(string line, bool safe, string because = "")
    {
        DayTwo.PartTwo([line]).Should().Be(safe ? 1 : 0, because);
    }
}