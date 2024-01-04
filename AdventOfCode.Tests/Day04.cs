namespace AdventOfCode.Tests;

[TestFixture]
public class Day04Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Task1_TestData()
    {
        const string testStr = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        var cards = testStr.Split("\n")
            .Select(Card.Parse)
            .Select(c => c.Points())
            .Sum();
        
        Assert.That(cards, Is.EqualTo(13));
    }

    [Test]
    public void Card_GenerateCopies()
    {
        const string testStr = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

        var cards = testStr.Split("\n")
            .Select(Card.Parse);

        var deck = Card.GenerateCopies(cards);

        var totalCopies = deck.Sum();
        
        Assert.That(totalCopies, Is.EqualTo(30));
    }

// Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    [Test]
    public void Card_ParseTestCard()
    {
        const string testStr = "Card   1:  4 33 89 61 95 36  5 30 26 55 | 15 33 28 36 93 57 26 13 95  4 18 79  6 87 60 66 69 67 19 42 22 61 78  5 58";

        var result = Card.Parse(testStr);
        
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(1));
            
            Assert.That(result.Numbers, Has.Count.EqualTo(10));
            Assert.That(result.Winners, Has.Count.EqualTo(25));
        });
    }

    [Test]
    public void Card_Matches()
    {
        const string testStr = "Card 1: 41 48 83  | 83 86";

        var card = Card.Parse(testStr);
        var result = card.Matches().ToList();
        
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result, Contains.Item(83));
    }
    
    [Test]
    public void Card_Power_1()
    {
        const string testStr = "Card 1: 41 48 83  | 83 86";

        var card = Card.Parse(testStr);
        var result = card.Points();
        

        Assert.That(result, Is.EqualTo(1));
    }
    
    [Test]
    public void Card_Power_0()
    {
        const string testStr = "Card 1: 41 48 | 83 86";

        var card = Card.Parse(testStr);
        var result = card.Points();

        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Card_Power_5()
    {
        const string testStr = "Card 1: 41 48 83 45 4  | 41 48 83 45 4";

        var card = Card.Parse(testStr);
        var result = card.Points();

        Assert.That(result, Is.EqualTo(16));
    }
    
}