namespace AdventOfCode.Tests;
using AdventOfCode;

public class Day02Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GameParser_Parses_Game()
    {
        const string testGameStr = "Game 16: 3 red, 3 green; 6 green, 4 red, 3 blue; 3 red, 4 blue; 4 blue, 2 red, 4 green";

        var result = GameParser.ParseGame(testGameStr);

        var expected = new Game { Rounds =
            [
                new Round { Red = 3, Green = 3 }, new Round { Red = 4, Green = 6, Blue = 3 },
                new Round { Red = 3, Blue = 4 }, new Round { Red = 2, Green = 4, Blue = 4 }
            ]
        };
        
        Assert.That(result.Rounds, Has.Count.EqualTo(expected.Rounds.Count));
    }

    [Test]
    public void GameParser_Parses_GameId()
    {
        const string testStr = "Game 16";

        var result = GameParser.ParseGameId(testStr);
        
        Assert.That(result, Is.EqualTo(16));
    }
    
    [Test]
    public void GameParser_Parses_A_Round()
    {
        const string testStr = "6 green, 4 red, 3 blue";

        var result = GameParser.ParseRound(testStr);
        
        Assert.That(result.Red, Is.EqualTo(4));
        Assert.That(result.Green, Is.EqualTo(6));
        Assert.That(result.Blue, Is.EqualTo(3));
    }
    
    [Test]
    public void GameParser_Parses_Incomplete_Round()
    {
        const string testStr = "6 green, 3 blue";

        var result = GameParser.ParseRound(testStr);
        Assert.Multiple(() =>
        {
            Assert.That(result.Green, Is.EqualTo(6));
            Assert.That(result.Blue, Is.EqualTo(3));
        });
    }

    [Test]
    public void GameValidator_IsValid()
    {
        var game = new Game { Id = 1, Rounds = { new Round { Red = 2, Green = 2, Blue = 2 } } };
        var validValues = new Round { Red = 11, Green = 11, Blue = 11 };
        
        var result = GameValidator.IsValid(game, validValues);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GameValidator_IsValidIncompleteRound()
    {
        var game = new Game { Id = 1, Rounds = { new Round { Red = 2, Blue = 2 } } };
        var validValues = new Round { Red = 11, Green = 11, Blue = 11 };
        
        var result = GameValidator.IsValid(game, validValues);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GameValidator_IsNotValid()
    {
        var game = new Game { Id = 1, Rounds = { new Round { Red = 2, Green = 2, Blue = 2 } } };
        var validValues = new Round { Red = 1, Green = 11, Blue = 11 };
        
        var result = GameValidator.IsValid(game, validValues);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GameValidator_GetValidGames()
    {
        var game1 = new Game { Id = 1, Rounds = { new Round { Red = 2, Green = 2, Blue = 2 } } };
        var game2 = new Game { Id = 2, Rounds = { new Round { Red = 2, Green = 2, Blue = 2 } } };
        var game3 = new Game { Id = 3, Rounds = { new Round { Red = 22, Green = 2, Blue = 2 } } };
        var validValues = new Round { Red = 11, Green = 11, Blue = 11 };

        var games = new List<Game> { game1, game2, game3 };
        
        var result = GameValidator.GetValidGames(games, validValues);
        
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public void GamePowerCalculator_CalculatePower()
    {
        // Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
        var game2 = new Game { Id = 3, Rounds = { new Round { Red = 20, Green = 8, Blue = 6 }, new Round { Red = 4, Green = 13, Blue = 5 }, new Round { Red = 1, Green = 5 } } };
        var expectedPower = new Round { Red = 20, Green = 13, Blue = 6 };

        var result = GamePowerCalculator.CalculatePower(game2);
        Assert.Multiple(() =>
        {
            Assert.That(result.Red, Is.EqualTo(expectedPower.Red));
            Assert.That(result.Blue, Is.EqualTo(expectedPower.Blue));
            Assert.That(result.Green, Is.EqualTo(expectedPower.Green));
            Assert.That(result.Power, Is.EqualTo(1560));
        });
    }
}