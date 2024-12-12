using AdventOfCode2024.DaySeven;
using FluentAssertions;

namespace Tests;

public class DaySevenTests
{
    [Fact]
    public void Test1()
    {
        const string input = """
                             190: 10 19
                             3267: 81 40 27
                             83: 17 5
                             156: 15 6
                             7290: 6 8 6 15
                             161011: 16 10 13
                             192: 17 8 14
                             21037: 9 7 18 13
                             292: 11 6 16 20
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DaySeven.PartOne(lines).Should().Be(3749);
    }

    [Fact]
    public void Test2()
    {
        const string input = """
                             190: 10 19
                             3267: 81 40 27
                             83: 17 5
                             156: 15 6
                             7290: 6 8 6 15
                             161011: 16 10 13
                             192: 17 8 14
                             21037: 9 7 18 13
                             292: 11 6 16 20
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DaySeven.PartTwo(lines).Should().Be(11387);
    }

    [Fact]
    public void EvaluatorTest()
    {
        LeftToRightEvaluator<int>.Evaluate("81 + 40 * 27").Should().Be(3267);
    }

    [Fact]
    public void PermutationTest()
    {
        var result = PermutationGenerator.GeneratePermutations("ABC".ToCharArray(), 5).ToArray();
        result.Should().HaveCount(243);
        result.Should().Contain("ABCAB");
    }
}