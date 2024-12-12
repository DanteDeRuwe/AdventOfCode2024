using AdventOfCode2024.DayNine;
using FluentAssertions;

namespace Tests;

public class DayNineTests
{
    [Fact]
    public void Test1()
    {
        const string input = "2333133121414131402";
        DayNine.PartOne(input).Should().Be(1928);
    }

    [Fact]
    public void Test2()
    {
        throw new NotImplementedException();
    }
}