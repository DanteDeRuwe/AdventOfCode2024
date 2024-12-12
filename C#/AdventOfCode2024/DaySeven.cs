using System.Globalization;
using System.Numerics;
using System.Text;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2024.DaySeven;

public static class DaySeven
{
    public static long PartOne(string[] lines)
    {
        var equations = Equations.FromLines(lines);
        return GetResult(equations, ['+', '*']);
    }

    public static long PartTwo(string[] lines)
    {
        var equations = Equations.FromLines(lines);
        return GetResult(equations, ['+', '*', '|']);
    }

    private static long GetResult(IEnumerable<Equation> equations, char[] possibleOperators)
    {
        return equations.Aggregate(0L, (acc, equation) =>
        {
            var canBeCorrect = PermutationGenerator
                .GeneratePermutations(possibleOperators, equation.Numbers.Length - 1)
                .Select(equation.CreateExpression)
                .Select(LeftToRightEvaluator<long>.Evaluate)
                .Any(res => res == equation.Result);

            return canBeCorrect ? acc + equation.Result : acc;
        });
    }
}

public static class Defaults
{
    public const StringSplitOptions SplitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
}

public static class LeftToRightEvaluator<T> where T : INumber<T>
{
    public static T Evaluate(string expression)
    {
        var tokens = expression.Split(" ", Defaults.SplitOptions);
        var stack = new Stack<string>(tokens.Reverse()); // Reverse the tokens to pop them in the correct order

        while (stack.Count > 1)
        {
            var left = T.Parse(stack.Pop(), CultureInfo.InvariantCulture);
            var operation = stack.Pop();
            var right = T.Parse(stack.Pop(), CultureInfo.InvariantCulture);

            var result = operation switch
            {
                "+" => (left + right).ToString(),
                "*" => (left * right).ToString(),
                "|" => $"{left}{right}", // Custom operator to concatenate numbers
                _ => throw new InvalidOperationException($"Invalid operator: {operation}")
            };
            stack.Push(result!); // Push the result back to the stack to be computed with the next number
        }

        return T.Parse(stack.Pop(), CultureInfo.InvariantCulture);
    }
}

public static class PermutationGenerator
{
    ///<summary>Generate all possible permutations of the operators</summary>
    /// <example> GeneratePermutations(['+', '*'], 3) returns 2^3 or 8 permutations: [[+++][++*][+**][***][**+][*++][*+*][+*+]] </example>
    public static IEnumerable<IEnumerable<char>> GeneratePermutations(char[] options, int length)
    {
        for (var i = 0; i < Math.Pow(options.Length, length); i++)
        {
            yield return BaseConverter.Convert(i, options).PadLeft(length, options[0]);
        }
    }
}

public static class BaseConverter
{
    /// <summary>Converts a number to a different base N using the provided base characters of length N</summary>
    /// <example>BaseConverter.Convert(8, [0,1]) -> 1111 (cf. binary)</example>
    public static string Convert(int value, char[] baseChars)
    {
        var sb = new StringBuilder();
        var targetBase = baseChars.Length;

        do
        {
            sb.Insert(0, baseChars[value % targetBase]);
            value = value / targetBase;
        } while (value > 0);

        return sb.ToString();
    }
}

public record Equation(long Result, long[] Numbers)
{
    public static Equation Parse(string line)
    {
        var parts = line.Split(':', Defaults.SplitOptions);
        return new Equation(long.Parse(parts[0]), parts[1].Split(' ', Defaults.SplitOptions).Select(long.Parse).ToArray());
    }

    public string CreateExpression(IEnumerable<char> operators) => 
        string.Join(' ', Numbers.SkipLast(1).Zip(operators, (n, o) => $"{n} {o}")) + $" {Numbers.Last()}";
}

public static class Equations
{
    public static IEnumerable<Equation> FromLines(string[] lines) => lines.Select(Equation.Parse);
}