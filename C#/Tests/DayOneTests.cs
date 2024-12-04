using FluentAssertions;

namespace Tests;

public class DayOneTests
{
    [Fact]
    public void Test1()
    {
        string[] lines =
        [
            "3   4",
            "4   3",
            "2   5",
            "1   3",
            "3   9",
            "3   3"
        ];
        DayOne.PartOne(lines).Should().Be(11);
    }
    
    [Fact]
    public void Test2()
    {
        string[] lines =
        [
            "3   4",
            "4   3",
            "2   5",
            "1   3",
            "3   9",
            "3   3"
        ];
        DayOne.PartTwo(lines).Should().Be(31);
    }
}