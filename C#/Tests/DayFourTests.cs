using FluentAssertions;

namespace Tests;

public class DayFourTests
{
    [Fact]
    public void Test1()
    {
        const string input = """
                             MMMSXXMASM
                             MSAMXMSMSA
                             AMXSXMAAMM
                             MSAMASMSMX
                             XMASAMXAMM
                             XXAMMXXAMA
                             SMSMSASXSS
                             SAXAMASAAA
                             MAMMMXMMMM
                             MXMXAXMASX
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayFour.PartOne(lines).Should().Be(18);
    }

    [Fact]
    public void Test2()
    {
        const string input = """
                             MMMSXXMASM
                             MSAMXMSMSA
                             AMXSXMAAMM
                             MSAMASMSMX
                             XMASAMXAMM
                             XXAMMXXAMA
                             SMSMSASXSS
                             SAXAMASAAA
                             MAMMMXMMMM
                             MXMXAXMASX
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayFour.PartTwo(lines).Should().Be(9);
    }
    
    [Fact]
    public void Test2_Custom0()
    {
        const string input = """
                             M*S
                             *A*
                             M*S
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayFour.PartTwo(lines).Should().Be(1);
    }
    
    [Fact]
    public void Test2_Custom1()
    {
        const string input = """
                             MASAMMAM
                             *A*A**A*
                             MASAMSAS
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayFour.PartTwo(lines).Should().Be(3);
    }
    
    
    [Fact]
    public void Test2_Custom2()
    {
        const string input = """
                             M*S
                             AAA
                             S*M
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayFour.PartTwo(lines).Should().Be(0);
    }
}