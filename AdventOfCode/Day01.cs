using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var totalSum =
            _input
                .Split("\n")
                .Select(line =>
                {
                    var digits = line.Where(char.IsDigit).Select(char.GetNumericValue);
                    return digits.First() * 10 + digits.Last();
                })
                .Sum();
        return new ValueTask<string>(totalSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var targets = new string[]
        {
            "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "0", "1", "2", "3", "4",
            "5", "6", "7", "8", "9"
        };

        var totalSum =
            _input
                .Split("\n")
               // .Take(1)
                .Select(line =>
                {
                    var matches = new List<(int, int)>();
                    for (var index = 0; index < targets.Length; index++)
                    {
                        var pattern = targets[index];
                        var r = new Regex(pattern);
                        var results = r.Matches(line);

                        foreach (Match result in results)
                        {
                            matches.Add((result.Index, index % 10));
                        }
                    }

                    matches.Sort((a, b) => a.Item1.CompareTo(b.Item1));

                    var num = matches.First().Item1 * 10 + matches.Last().Item1;
                    return num;
                })
                .Sum();
        return new ValueTask<string>(totalSum.ToString());
    }
}