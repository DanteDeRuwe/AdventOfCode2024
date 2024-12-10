// ReSharper disable once CheckNamespace

namespace AdventOfCode2024.DaySix;

public static class DaySix
{
    public static int PartOne(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();
        var guardPosition = cells.FindGuard();

        var visited = new HashSet<Cell> { guardPosition.Position };

        while (guardPosition.NextPosition(cells) is GuardPosition nextPosition) // Move guard until we fall off the map
        {
            guardPosition = nextPosition;
            visited.Add(guardPosition.Position);
        }

        return visited.Count;
    }

    public static int PartTwo(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();
        var startPosition = cells.FindGuard();

        var result = new HashSet<Cell>();

        // I tried everything else, but then said f*ck it and brute forced this crap
        foreach (var cell in cells.GetOpen())
        {
            // Try make this cell a wall
            cells[cell.Y][cell.X] = cell with { Value = '#' };

            var visited = new HashSet<GuardPosition>([startPosition]);
            var guardPosition = startPosition;
            while (guardPosition.NextPosition(cells) is GuardPosition nextPosition) // Move guard until we fall off the map
            {
                guardPosition = nextPosition;
                if (visited.Add(guardPosition)) continue; // If new position, continue
                result.Add(cell); // If we've been here before -> it's a loop. Add the cell that caused the loop to the result
                break;
            }

            // Reset cell to original value
            cells[cell.Y][cell.X] = cell;
        }

        return result.Count;
    }
}

public record Cell(char Value, int X, int Y)
{
    public Cell? GetNeighborInDirection(Cell[][] cells, Direction direction) => cells.Get(X + direction.X, Y + direction.Y);
}

public record GuardPosition(Cell Position, Direction Direction)
{
    public GuardPosition? NextPosition(Cell[][] cells)
    {
        if (Position.GetNeighborInDirection(cells, Direction) is not Cell next) return null;

        return next.Value == '#'
            ? this with { Direction = Direction.RotateRight() }
            : this with { Position = next };
    }
}

public static class Cells
{
    public static Cell[][] FromLines(string[] lines) =>
        lines.Select((line, y) => line.Select((c, x) => new Cell(c, x, y)).ToArray()).ToArray();

    public static Cell? Get(this Cell[][] cells, int x, int y)
    {
        try
        {
            return cells[y][x];
        }
        catch
        {
            return null;
        }
    }

    public static IEnumerable<Cell> GetOpen(this Cell[][] cells) =>
        cells.SelectMany(c => c).Where(c => c.Value == '.');

    public static GuardPosition FindGuard(this Cell[][] cells)
    {
        var cell = cells.SelectMany(c => c).Single(c => c.Value is '^' or 'v' or '<' or '>');
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