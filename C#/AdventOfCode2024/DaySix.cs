// ReSharper disable once CheckNamespace

namespace AdventOfCode2024.DaySix;

public static class DaySix
{
    public static int PartOne(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();
        var (guardCell, direction) = Cells.FindGuard(cells);

        var visited = new HashSet<Cell> { guardCell };

        while (guardCell.GetNeighborInDirection(cells, direction) is Cell neighbor)
        {
            if (neighbor.Value == '#')
            {
                direction = direction.RotateRight();
                continue;
            }

            guardCell = neighbor;
            visited.Add(guardCell);
        }

        return visited.Count;
    }

    public static int PartTwo(string input)
    {
        throw new NotImplementedException();
    }
}

public record Cell(char Value, int X, int Y)
{
    public Cell? GetNeighborInDirection(IEnumerable<Cell> cells, Direction direction) =>
        cells.SingleOrDefault(c => X + direction.X == c.X && Y + direction.Y == c.Y);
}

public record Guard(Cell CurrentPosition, Direction Direction);

public static class Cells
{
    public static IEnumerable<Cell> FromLines(string[] lines) =>
        lines.SelectMany((line, y) => line.Select((c, x) => new Cell(c, x, y)));

    public static Guard FindGuard(Cell[] cells)
    {
        var cell = cells.Single(c => c.Value is '^' or 'v' or '<' or '>');
        return cell.Value switch
        {
            '^' => new Guard(cell, new Direction(0, -1)),
            'v' => new Guard(cell, new Direction(0, 1)),
            '<' => new Guard(cell, new Direction(-1, 0)),
            '>' => new Guard(cell, new Direction(1, 0)),
            _ => throw new ArgumentException()
        };
    }
}

public record Direction(int X, int Y)
{
    public Direction RotateRight() => new(-Y, X);
}