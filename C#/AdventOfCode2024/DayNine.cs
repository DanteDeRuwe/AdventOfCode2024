using System.Collections;
using System.Collections.Immutable;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2024.DayNine;

public static class DayNine
{
    public static long PartOne(string input)
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
            if (blocks.IsEmpty) break; // No more blocks to add

            // Add first block
            var first = blocks.RemoveFirst();
            result.AddRange(first);

            if (blocks.IsEmpty) break; // No more blocks to add

            // Fill this free space with blocks from the end
            var remainingSpacesToFill = freeSpacesToFill;
            while (true)
            {
                if (blocks.IsEmpty) break; // No more blocks to add
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


    // TODO instead of manipulating lists, just check in one list all the numbers, and add them to a result without removing them??

    public static long PartTwo(string input)
    {
        var allInput = input.Select(c => int.Parse(c.ToString())).ToArray();


        var blocks = allInput.Index().Where(x => x.Index % 2 == 0).Select(x => Enumerable.Repeat(x.Index / 2, x.Item)).ToList();
        var freeSpaces = allInput.Index().Where(x => x.Index % 2 == 1).Select(x => Enumerable.Repeat(-1, x.Item));
        var representation = blocks
            .Zip(freeSpaces, (b, f) => new[] { b, f })
            .SelectMany(x => x)
            .Append(blocks.Last())
            .Select(x => x.ToList())
            .ToList();

        DoPartTwo(representation);

        
        var result = representation.SelectMany(x => x).Select(x => x == -1 ? 0 : x).ToArray(); // convert -1 to 0

        return Checksum.Calculate(result);
    }

    private static void DoPartTwo(List<List<int>> representation)
    {
        var reversedBlocksWithOriginalIndex = representation.Index().Where(x => x.Item.Count > 0 && x.Item.All(xx => xx != -1)).Reverse().ToArray();

        foreach (var (i, block) in reversedBlocksWithOriginalIndex)
        {
         
            (int Index, List<int> Item) firstAvailableSpot;
            try
            {
                firstAvailableSpot = representation.Index().First(x => x.Item.Count(xx => xx == -1) >= block.Count);
                if(firstAvailableSpot.Index > i) continue; // has to be to the left of file
            }
            catch
            {
                continue; // No spot available
            }

            // Spot found!

            // Set this block to empty spaces
            representation[i] = Enumerable.Repeat(-1, block.Count).ToList();

            // Add block to the first available spot
            var allocatedSpaces = firstAvailableSpot.Item.Count(xx => xx != -1);
            var freeSpacesLeft = firstAvailableSpot.Item.Count - block.Count - allocatedSpaces;
            var freeSpaceBecomes = firstAvailableSpot.Item.Take(allocatedSpaces)
                .Concat(block)
                .Concat(Enumerable.Repeat(-1, freeSpacesLeft))
                .ToList();
            
            representation[firstAvailableSpot.Index] = freeSpaceBecomes;
        }
    }
}

public static class Checksum
{
    public static long Calculate(IEnumerable<int> numbers) => numbers.Index().Sum(x => (long)x.Index * x.Item);
}

public class Deque<T>(IEnumerable<T>? items = null) : IEnumerable<T>
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
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class Extensions
{
    public static Deque<T> ToDeque<T>(this IEnumerable<T> items) => new(items);
}