namespace AdventOfCode;

public class Round
{
    public int Red;
    public int Green;
    public int Blue;

    public int Power => Red * Green * Blue;
}

public class Game
{
    public int Id;
    public List<Round> Rounds = [];
}

// Game Pattern: "Game <id>: Round+"
public static class GameParser
{
    private static (string, string) SplitGameHeaderAndRounds(string input)
    {
        var strArr = input.Split(":");

        return (strArr[0], strArr[1]);
    }

    public static Game ParseGame(string input)
    {
        var game = new Game();

        var (gameHeader, rounds) = SplitGameHeaderAndRounds(input);

        game.Id = ParseGameId(gameHeader);
        game.Rounds = ParseRounds(rounds);

        return game;
    }

    // 6 green, 4 red, 3 blue
    public static Round ParseRound(string roundStr)
    {
        var round = new Round();
       var hands = roundStr.Split(",").Select(s => s.Trim());
       foreach (var hand in hands)
       {
           var count = hand.Split(" ")[0];
           var handType = hand.Split(" ")[1];

           var _ =handType.Trim() switch
           {
               "red" => round.Red = int.Parse(count),
               "green" => round.Green = int.Parse(count),
               "blue" => round.Blue = int.Parse(count),
               _ => throw new ArgumentOutOfRangeException()
           };
       }

       return round;
    }

    public static List<Round> ParseRounds(string rounds)
    {
        return rounds.Split(";").AsEnumerable().Select(ParseRound).ToList();
    }

    public static int ParseGameId(string gameHeader)
    {
        var idStr = gameHeader.Split(" ");

        return int.Parse(idStr[1]);
    }
}

public static class GameValidator
{
    public static bool IsValid(Game game, Round rule)
    {
        return game.Rounds.All(round => round.Red <= rule.Red && round.Green <= rule.Green && round.Blue <= rule.Blue);
    }
    
    public static List<Game> GetValidGames(List<Game> games, Round rule)
    {
        var validGames = games.Where(g =>IsValid(g, rule));

        return validGames.ToList();

    }
}

public static class GamePowerCalculator
{
    public static Round CalculatePower(Game game)
    {
        var power = new Round();
        foreach (var round in game.Rounds)   
        {
            if (round.Red > power.Red)
            {
                power.Red = round.Red;
            }
            if (round.Green > power.Green)
            {
                power.Green = round.Green;
            }
            if (round.Blue > power.Blue)
            {
                power.Blue = round.Blue;
            }
        }

        return power;
    }
}

public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    // 12 red cubes, 13 green cubes, and 14 blue cubes
    public override ValueTask<string> Solve_1()
    {
        var games = _input.Split("\n").Select(GameParser.ParseGame).ToList();
        var validRound = new Round { Red = 12, Green = 13, Blue = 14 };
        
        var allValidGames = GameValidator.GetValidGames(games, validRound);

        var totalValue = allValidGames.Sum(g => g.Id);

        return new ValueTask<string>(totalValue.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var res =_input.Split("\n")
                            .Select(GameParser.ParseGame)
                            .Select(GamePowerCalculator.CalculatePower)
                            .Sum(p=> p.Power);

        return new ValueTask<string>(res.ToString());
    }
}