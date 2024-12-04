using FluentAssertions;

namespace Tests;

public class DayThreeTests
{
    [Fact]
    public void Test1()
    {
        const string input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        DayThree.PartOne(input).Should().Be(161);
    }

    [Fact]
    public void Test2()
    {
        const string input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        DayThree.PartTwo(input).Should().Be(48);
    }
}