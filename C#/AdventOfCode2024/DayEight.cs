using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2024.DayEight;

public static class DayEight
{
    public static int PartOne(string[] lines)
    {
        var grid = Grid.FromLines(lines);

        var antennae = grid.Cells.SelectMany(c => c)
            .Where(c => !c.IsOpen)
            .GroupBy(c => c.Value)
            .Select(g => new Antenna(g.Key, g.ToArray()));

        var allAntinodes = new HashSet<Position>();
        foreach (var antenna in antennae.Where(a => a.Instances.Length > 1))
        {
            foreach (var instance in antenna.Instances)
            {
                var antiNodes = antenna.Instances
                    .Where(other => other != instance) // don't compare to itself
                    .Select(other => other.Position.GetDistanceTo(instance.Position)) //get distance from other instance to this one
                    .Select(distance => instance.Position.Move(distance)) // move same distance away from this instance
                    .Where(newPos => newPos.IsWithinBounds(grid)); // see if it's within bounds

                allAntinodes.UnionWith(antiNodes);
            }
        }

        return allAntinodes.Count;
    }

    public static int PartTwo(string[] lines)
    {
        var grid = Grid.FromLines(lines);

        var antennae = grid.Cells.SelectMany(c => c)
            .Where(c => !c.IsOpen)
            .GroupBy(c => c.Value)
            .Select(g => new Antenna(g.Key, g.ToArray()));

        var allAntinodes = new HashSet<Position>();
        foreach (var antenna in antennae.Where(a => a.Instances.Length > 1))
        {
            foreach (var instance in antenna.Instances)
            {
                var antiNodes = antenna.Instances
                    .Where(other => other != instance) // don't compare to itself
                    .Select(other => other.Position.GetDistanceTo(instance.Position)) //get distance from other instance to this one
                    .Select(distance => distance.ScaleDown()) // scale down to smallest possible vector (= direction)
                    .SelectMany(vector =>
                    {
                        var result = new List<Position>();
                        var pos = instance.Position;
                        while (pos.IsWithinBounds(grid))
                        {
                            result.Add(pos); // Doing this first will this instance to the list too, but that's what we want
                            pos = pos.Move(vector);
                        }

                        return result;
                    });

                allAntinodes.UnionWith(antiNodes);
            }
        }

        return allAntinodes.Count;
    }
}

public record Antenna(char Name, Cell[] Instances);

[DebuggerDisplay("({X},{Y})")]
public record Position(int X, int Y)
{
    public Position Move(Vector vector) => new(X + vector.X, Y + vector.Y);
    public Vector GetDistanceTo(Position other) => new(other.X - X, other.Y - Y);

    public bool IsWithinBounds(Grid grid) => this is { X: >= 0, Y: >= 0 } && X < grid.SizeX && Y < grid.SizeY;
}

public record Cell(char Value, Position Position)
{
    public bool IsOpen => Value == '.';
}

public record Grid(Cell[][] Cells)
{
    public int SizeX => Cells[0].Length;
    public int SizeY => Cells.Length;

    public static Grid FromLines(string[] lines) =>
        new(lines.Select((line, y) => line.Select((c, x) => new Cell(c, new Position(x, y))).ToArray()).ToArray());
}

public record Vector(int X, int Y)
{
    /// <summary>Scales the vector down to the smallest possible integer vector with the same direction (i.e. gcd)</summary>
    /// <example>Vector(-6, 8).ScaleDown() == Vector(-3, 4)</example>
    public Vector ScaleDown()
    {
        var (absX, absY) = (Math.Abs(X), Math.Abs(Y));
        var greatest = Math.Max(absX, absY);
        var gcd = Enumerable.Range(2, greatest - 1).Reverse().FirstOrDefault(f => absX % f == 0 && absY % f == 0, defaultValue: 1);
        return new Vector(X / gcd, Y / gcd);
    }
}