using System.Collections.Immutable;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2024.DayNine;

public static class DayNine
{
    public static int PartOne(string input)
    {
        var allInput = input.Select(c => int.Parse(c.ToString()));

        var isBlock = allInput.Index()
            .GroupBy(x => x.Index % 2 == 0, x => x.Item)
            .ToImmutableSortedDictionary(x => x.Key, x => x.AsEnumerable());

        var freeSpaces = isBlock[false];
        var blocks = isBlock[true].Index().Select(x => Enumerable.Repeat(x.Index, x.Item)).ToDeque();

        var result = new List<int>();
        foreach (var freeSpacesToFill in freeSpaces)
        {
            // Add first block
            var first = blocks.RemoveFirst();
            result.AddRange(first);

            if (blocks.IsEmpty) break; // No more blocks to add

            // Fill this free space with blocks from the end
            var remainingSpacesToFill = freeSpacesToFill;
            while (true)
            {
                var block = blocks.RemoveLast().ToArray();

                if (block.Length > remainingSpacesToFill)
                {
                    var take = block.Take(remainingSpacesToFill);
                    blocks.AddLast(block.Skip(remainingSpacesToFill)); // Add back the excess to the blocks
                    result.AddRange(take);
                    break; // Now no more free space to fill
                }

                result.AddRange(block);
                remainingSpacesToFill -= block.Length;
            }
        }

        return Checksum.Calculate(result);
    }

    public static int PartTwo(string input)
    {
        throw new NotImplementedException();
    }
}

public static class Checksum
{
    public static int Calculate(IEnumerable<int> numbers) => numbers.Index().Sum(x => x.Index * x.Item);
}

public record BlockItem(int Number, int Times);

public class Deque<T>(IEnumerable<T>? items = null)
{
    private readonly LinkedList<T> _list = items is null ? new() : new(items);

    public void AddFirst(T item) => _list.AddFirst(item);
    public void AddLast(T item) => _list.AddLast(item);

    public T RemoveFirst()
    {
        var first = _list.First;
        _list.RemoveFirst();
        return first.Value;
    }

    public T RemoveLast()
    {
        var last = _list.Last;
        _list.RemoveLast();
        return last.Value;
    }

    public T PeekFirst() => _list.First.Value;
    public T PeekLast() => _list.Last.Value;
    public int Count => _list.Count;
    public bool IsEmpty => _list.Count == 0;
}

public static class Extensions
{
    public static Deque<T> ToDeque<T>(this IEnumerable<T> items) => new(items);
}