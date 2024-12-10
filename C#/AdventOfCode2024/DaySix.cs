// ReSharper disable once CheckNamespace

namespace AdventOfCode2024.DaySix;

public static class DaySix
{
    public static int PartOne(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();
        var guardPosition = Cells.FindGuard(cells);

        var visited = new HashSet<Cell> { guardPosition.Position };

        while (guardPosition.NextPosition(cells) is GuardPosition nextPosition)
        {
            guardPosition = nextPosition;
            visited.Add(guardPosition.Position);
        }

        return visited.Count;
    }

    public static int PartTwo(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();
        var guardPosition = Cells.FindGuard(cells);

        var result = new HashSet<Cell>();

        // Warning, this loop is very slow. I did not find the willpower to optimize it.
        while (true)
        {
            var nextPosition = guardPosition.NextPosition(cells);
            if (nextPosition is null) break; // we fell off the map

            if (!result.Contains(nextPosition.Position))
            {
                // check if setting obstacle in front of us would lead to a loop
                var newCells = cells.Select(c => c == nextPosition.Position ? c with { Value = '#' } : c).ToArray();

                if (guardPosition.LeadsToLoop(newCells))
                {
                    var added = result.Add(nextPosition.Position);
                    if (!added) break; // we've been here before
                }
            }

            guardPosition = nextPosition; //one step forward and try again
        }

        return result.Count;
    }
}

public record Cell(char Value, int X, int Y)
{
    public Cell? GetNeighborInDirection(IEnumerable<Cell> cells, Direction direction) =>
        cells.SingleOrDefault(c => X + direction.X == c.X && Y + direction.Y == c.Y);
}

public record GuardPosition(Cell Position, Direction Direction)
{
    public GuardPosition? NextPosition(IEnumerable<Cell> cells)
    {
        if (Position.GetNeighborInDirection(cells, Direction) is not Cell next) return null;

        return next.Value == '#'
            ? this with { Direction = Direction.RotateRight() }
            : this with { Position = next };
    }
}

public static class LoopChecker
{
    public static bool LeadsToLoop(this GuardPosition guardPosition, IList<Cell> cells)
    {
        var visited = new HashSet<GuardPosition>([guardPosition]);

        while (guardPosition.NextPosition(cells) is GuardPosition nextPosition)
        {
            guardPosition = nextPosition;
            if (!visited.Add(guardPosition)) return true; // We've been here before with same direction!
        }

        return false; // we fell off the map
    }
}

public static class Cells
{
    public static IEnumerable<Cell> FromLines(string[] lines) =>
        lines.SelectMany((line, y) => line.Select((c, x) => new Cell(c, x, y)));

    public static GuardPosition FindGuard(Cell[] cells)
    {
        var cell = cells.Single(c => c.Value is '^' or 'v' or '<' or '>');
        return cell.Value switch
        {
            '^' => new GuardPosition(cell, new Direction(0, -1)),
            'v' => new GuardPosition(cell, new Direction(0, 1)),
            '<' => new GuardPosition(cell, new Direction(-1, 0)),
            '>' => new GuardPosition(cell, new Direction(1, 0)),
            _ => throw new ArgumentException()
        };
    }
}

public record Direction(int X, int Y)
{
    public Direction RotateRight() => new(-Y, X);
}