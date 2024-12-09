using System.Collections.Immutable;

// Console.WriteLine(DayFive.PartOne(File.ReadAllText("inputs/5.txt")));
// Console.WriteLine(DayFive.PartTwo(File.ReadAllText("inputs/5.txt")));

public static class DayFive
{
    public static int PartOne(string input)
    {
        var (rules, updates) = ParseInput(input);

        return updates
            .Where(update => update.PassesAll(rules))
            .Sum(update => update.MiddleNumber);
    }

    public static int PartTwo(string input)
    {
        var (rules, updates) = ParseInput(input);

        return updates
            .Where(update => update.FailsAny(rules))
            .Select(update => update.Correct(rules))
            .Sum(update => update.MiddleNumber);
    }

    private static (IList<Rule>, IList<Update>) ParseInput(string input)
    {
        var parts = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var rules = parts[0].Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x =>
            {
                var ruleParts = x.Split("|").Select(int.Parse).ToArray();
                return new Rule(ruleParts[0], ruleParts[1]);
            })
            .ToImmutableList();

        var updates = parts[1].Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Update(x.Split(',').Select(int.Parse).ToImmutableList()))
            .ToImmutableList();

        return (rules, updates);
    }
}

record Rule(int Before, int After)
{
    public bool AppliesTo(IList<int> numbers) => numbers.Contains(Before) && numbers.Contains(After);
    public bool IsCorrectFor(IList<int> numbers) => numbers.IndexOf(Before) < numbers.IndexOf(After);
}

record Update(IList<int> Numbers)
{
    public int MiddleNumber => Numbers[Numbers.Count / 2];

    public Update Correct(IEnumerable<Rule> rules)
    {
        var relevantRules = FilterRules(rules).ToImmutableList();
        var numbers = Numbers.ToList();

        while (relevantRules.Any(r => !r.IsCorrectFor(numbers)))
        {
            foreach (var rule in relevantRules)
            {
                var beforeIndex = numbers.IndexOf(rule.Before);
                var afterIndex = numbers.IndexOf(rule.After);
                if (beforeIndex < afterIndex) continue; // Already correct

                // Swap the numbers
                numbers[beforeIndex] = rule.After;
                numbers[afterIndex] = rule.Before;
            }
        }

        return new Update(numbers);
    }

    public IEnumerable<Rule> FilterRules(IEnumerable<Rule> rules) => rules.Where(r => r.AppliesTo(Numbers));
    public bool PassesAll(IEnumerable<Rule> rules) => FilterRules(rules).All(r => r.IsCorrectFor(Numbers));
    public bool FailsAny(IEnumerable<Rule> rules) => FilterRules(rules).Any(r => !r.IsCorrectFor(Numbers));
}