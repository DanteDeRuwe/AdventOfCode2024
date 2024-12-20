﻿using AdventOfCode2024.DayEight;
using FluentAssertions;
using Xunit.Sdk;

namespace Tests;

public class DayEightTests
{
    [Fact]
    public void Test1()
    {
        const string input = """
                             ............
                             ........0...
                             .....0......
                             .......0....
                             ....0.......
                             ......A.....
                             ............
                             ............
                             ........A...
                             .........A..
                             ............
                             ............
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayEight.PartOne(lines).Should().Be(14);
    }

    [Fact]
    public void Test2()
    {
        const string input = """
                             ............
                             ........0...
                             .....0......
                             .......0....
                             ....0.......
                             ......A.....
                             ............
                             ............
                             ........A...
                             .........A..
                             ............
                             ............
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayEight.PartTwo(lines).Should().Be(34);
    }
    
    [Fact]
    public void Test2_2()
    {
        const string input = """
                             ....0.......
                             ......A.....
                             ......A.....
                             """;

        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        DayEight.PartTwo(lines).Should().Be(3);
    }
}