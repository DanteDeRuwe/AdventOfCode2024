using AdventOfCode2024.DaySix;
using FluentAssertions;

namespace Tests;

public class DaySixTests
{
    [Fact]
    public void Test1()
    {
        const string input = """
                             ....#.....
                             .........#
                             ..........
                             ..#.......
                             .......#..
                             ..........
                             .#..^.....
                             ........#.
                             #.........
                             ......#...
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DaySix.PartOne(lines).Should().Be(41);
    }

    [Fact]
    public void Test2()
    {
        const string input = """
                             ....#.....
                             .........#
                             ..........
                             ..#.......
                             .......#..
                             ..........
                             .#..^.....
                             ........#.
                             #.........
                             ......#...
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DaySix.PartTwo(lines).Should().Be(6);
    }

    [Fact]
    public void NextPosition()
    {
        const string input = """
                             .........
                             ....^....
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);


        var cells = Cells.FromLines(lines);
        var pos = Cells.FindGuard(cells);

        var move = pos.NextPosition(cells);
        move.Should().NotBeNull();
        move!.NextPosition(cells).Should().BeNull();
    }
}