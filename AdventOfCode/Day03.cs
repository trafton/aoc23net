namespace AdventOfCode;

// Here is an example engine schematic:
//
//     467..114..
//     ...*......
//     ..35..633.
//     ......#...
//     617*......
//     .....+.58.
//     ..592.....
//     ......755.
//     ...$.*....
//     .664.598..

using System.Text.RegularExpressions;




public class Day03 : BaseDay
{
    
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }
    
    public class GetPartsResult
    {
        public IEnumerable<int> ValidParts = null!;
        public int GearRatio;
    }
    
    private GetPartsResult GetParts(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var lineLength = lines[0].Length;

        var symbolsRegex = new Regex("[^0-9.\r\n]");
        var starRegex = new Regex("[*]");
        var numbersRegex = new Regex(@"\d+");
        
        var potentialGears = new Dictionary<string, List<int>>();
        var validPartNumbers = new List<int>();

        // Get all numbers, along with their line number, start index and end index relative to the line that they are on.
        var numbers = lines.SelectMany(
                (line, lineNumber) => numbersRegex.Matches(line)
                    .Select(x => new
                    {
                        Value = int.Parse(x.Value),
                        LineNumber = lineNumber,
                        StartIndex = x.Index,
                        x.Length,
                        EndIndex = x.Index + x.Length
                    }));

        foreach (var number in numbers)
        {
            // Build line scan range
            var start = Math.Clamp(number.StartIndex - 1, 0, lineLength);
            var end = Math.Clamp(number.EndIndex + 1, 0, lineLength);
            var length = end - start;

            // check current line and above and below if they exist
            var lineNumbersToCheck = new List<int>(3) { number.LineNumber };
            if (number.LineNumber > 0) lineNumbersToCheck.Add(number.LineNumber - 1);
            if (number.LineNumber + 1 < lines.Length) lineNumbersToCheck.Add(number.LineNumber + 1);

            foreach (var lineNumber in lineNumbersToCheck)
            {
                var range = lines[lineNumber].Substring(start, length);
                if (symbolsRegex.Match(range).Success)
                {
                    validPartNumbers.Add(number.Value);
                }

                var starMatch = starRegex.Match(range);
                if (starMatch.Success)
                {
                    var potentialGearKey = $"{start + starMatch.Index},{lineNumber}";
                    if (potentialGears.ContainsKey(potentialGearKey))
                    {
                        potentialGears[potentialGearKey].Add(number.Value);
                    }
                    else
                    {
                        potentialGears[potentialGearKey] = [number.Value];
                    }
                }
            }
        }

        // A particular star is only a gear if exactly two part numbers are touching it.
        var gears = potentialGears
            .Where(x => x.Value.Count == 2)
            .Select(x => x.Value);

        return new GetPartsResult
        {
            ValidParts = validPartNumbers,
            GearRatio = gears.Select(x => x[0] * x[1]).Sum()
        };
    }
    
    //     In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right).
    //     Every other number is adjacent to a symbol and so is a part number; their sum is 4361.
    //
    //     Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?
    
    //     - For each number, check to see if it is next to a symbol
    public override ValueTask<string> Solve_1()
    {
        var result = GetParts(InputFilePath).ValidParts.Sum();

        return new ValueTask<string>(result.ToString());
    }

    // A gear is any * symbol that is adjacent to exactly two part numbers.
    // Its gear ratio is the result of multiplying those two numbers together.
    // 
    // This time, you need to find the gear ratio of every gear and add them all up
    // so that the engineer can figure out which gear needs to be replaced.
    public override ValueTask<string> Solve_2()
    {
        var result = GetParts(InputFilePath).GearRatio;

        return new ValueTask<string>(result.ToString());
    }
}