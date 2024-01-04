namespace AdventOfCode;

public class Card
{
    public int Id { get; private set; }
    public HashSet<int> Numbers = [];
    public HashSet<int> Winners = [];

    public IEnumerable<int> Matches()
    {
        return Winners.Intersect(Numbers);
    }

    public int Points()
    {
        var matchCount = Matches().Count();
        return matchCount switch
        {
            0 => 0,
            _ => (int)Math.Pow(2, matchCount - 1)
        };
    }

    public static Card Parse(string input)
    {
        Console.WriteLine(input);
        var cardIdStr = input.Split(":")[0];
        var cardInfoStr = input.Split(":")[1];

        var cardNumberStr = cardInfoStr.Split(" | ", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        var cardNums = cardNumberStr.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n.Trim()));

        var cardWinnerStr = cardInfoStr.Split(" | ", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
        var cardWinners = cardWinnerStr.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(n => int.Parse(n.Trim()));

        var cardId = int.Parse(cardIdStr.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);

        return new Card
        {
            Id = cardId,
            Numbers = cardNums.ToHashSet(),
            Winners = cardWinners.ToHashSet()
        };
    }

    public static IEnumerable<int> GenerateCopies(IEnumerable<Card> cards)
    {
        var cardCount = cards.Count();

        var deck = Enumerable.Repeat(1, cardCount).ToArray();

        foreach (var card in cards)
        {
            var currCards = deck[card.Id - 1];
            var matchCount = card.Matches().Count();
            foreach (var j in Enumerable.Range(0, currCards))
            {
                for (var i = 0; i < matchCount; i++)
                {
                    deck[card.Id + i]++;
                }
            }
        }

        return deck;
    }
}

public class Day04 : BaseDay
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var cards = _input.Split("\n")
            .Select(Card.Parse)
            .Select(c => c.Points())
            .Sum();

        return new ValueTask<string>(cards.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var cards = _input.Split("\n")
            .Select(Card.Parse);

        var deck = Card.GenerateCopies(cards);


        return new ValueTask<string>(deck.Sum().ToString());
    }
}