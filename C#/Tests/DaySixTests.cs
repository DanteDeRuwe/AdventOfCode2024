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
    public void LoopCheckerTest()
    {
        const string input = """
                             ....#.....
                             .........#
                             ...#^.....
                             ........#.
                             """;
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var cells = Cells.FromLines(lines).ToArray();
        var guardPosition = Cells.FindGuard(cells);
        
        guardPosition.LeadsToLoop(cells).Should().BeTrue();
    }
}