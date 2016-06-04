using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace musin_rustam
{
    public class musin_rustam
    {
        public static void Main(string[] args)
        {
            var hanabi = new HanabiInteractive();
            var gameResults = ReadLines().Select(hanabi.MakeTurn).Where(result => result != null);
            foreach (var result in gameResults)
                Console.WriteLine(result);
            Console.Out.Flush();
        }

        private static IEnumerable<string> ReadLines()
        {
            string s;
            while ((s = Console.ReadLine()) != null)
                yield return s;
        }
    }

    public class HanabiInteractive
    {
        public static int CardCount => 5;

        private int Turn { get; set; }
        private int Score { get; set; }
        private int WithRisk { get; set; }
        private bool Finished { get; set; }

        private Player FirstPlayer { get; }
        private Player SecondPlayer { get; }
        private bool FirstPlayerTurns { get; set; }
        private Player CurrentPlayer => FirstPlayerTurns ? FirstPlayer : SecondPlayer;
        private Player NextPlayer => FirstPlayerTurns ? SecondPlayer : FirstPlayer;

        private Deck Deck { get; }
        private Dictionary<Color, int> Table { get; }
        private Dictionary<Type, Action<GameAction>> Actions { get; }


        public HanabiInteractive()
        {
            FirstPlayer = new Player();
            SecondPlayer = new Player();
            Deck = new Deck();
            Table = new Dictionary<Color, int>();
            Actions = new Dictionary<Type, Action<GameAction>>();
            InitActions();
        }

        private void InitActions()
        {
            Actions[typeof (StartNewGameAction)] = action => StartNewGame((StartNewGameAction) action);
            Actions[typeof (PlayCardAction)] = action => PlayCard((PlayCardAction) action);
            Actions[typeof (DropCardAction)] = action => DropCard((DropCardAction) action);
            Actions[typeof (TellCardPropertyAction)] = action => TellCardProperty((TellCardPropertyAction) action);
        }

        private void ResetPlayers()
        {
            FirstPlayer.Reset();
            SecondPlayer.Reset();
        }

        private void ResetTable()
        {
            Table[Color.Red] = 0;
            Table[Color.Green] = 0;
            Table[Color.Blue] = 0;
            Table[Color.White] = 0;
            Table[Color.Yellow] = 0;
        }

        private void ResetGame()
        {
            Turn = 0;
            Score = 0;
            WithRisk = 0;
            Finished = false;
            FirstPlayerTurns = true;
            ResetPlayers();
            ResetTable();
        }

        private void InitDeck(IEnumerable<Card> cards)
        {
            Deck.Reset();
            Deck.Fill(cards);
        }

        private void InitPlayer(Player player)
        {
            for (var i = 0; i < CardCount; i++)
                player.AddCard(Deck.PollCard());
        }

        private void InitPlayers()
        {
            InitPlayer(FirstPlayer);
            InitPlayer(SecondPlayer);
        }

        private void SwapPlayers()
        {
            FirstPlayerTurns = !FirstPlayerTurns;
        }

        private void CheckIfDeckIsEmpty()
        {
            if (Deck.IsEmpty)
                throw new GameOverException("Deck is empty!");
        }

        private void CheckIfTableIsFilled()
        {
            if (Table.Values.Min() == 5)
                throw new GameOverException("Table is filled!");
        }

        public GameResult MakeTurn(string command)
        {
            GameResult result = null;
            try
            {
                var action = GameAction.Parse(command);
                if (!(action is StartNewGameAction))
                {
                    if (Finished) return null;
                    if (++Turn > 1) SwapPlayers();
                }
                Actions[action.GetType()](action);
                CheckIfDeckIsEmpty();
                CheckIfTableIsFilled();
            }
            catch (GameOverException e)
            {
                Finished = true;
                result = new GameResult(Turn, Score, WithRisk, e);
            }
            
            return result;
        }

        private void StartNewGame(StartNewGameAction action)
        {
            ResetGame();
            InitDeck(action.Deck);
            InitPlayers();
        }

        private void PlayCard(PlayCardAction action)
        {
            var card = CurrentPlayer.RemoveCard(action.CardNumber);
            var risky = IsRisky(card);
            PutCard(card);
            if (risky) WithRisk++;
            CurrentPlayer.AddCard(Deck.PollCard());
        }

        private void PutCard(Card card)
        {
            if (++Table[card.Color] != card.Rank)
                throw new GameOverException("Wrong rank!");
            Score++;
        }

        private bool IsRisky(Card card)
        {
            return (
                from color in card.AvailableColors
                from rank in card.AvailableRanks
                where IsRisky(color, rank)
                select true).Any();
        }

        private bool IsRisky(Color color, int rank)
        {
            return Table[color] + 1 != rank;
        }

        private void DropCard(DropCardAction action)
        {
            CurrentPlayer.RemoveCard(action.CardNumber);
            CurrentPlayer.AddCard(Deck.PollCard());
        }

        private void TellCardProperty(TellCardPropertyAction action)
        {
            var propertyName = action.PropertyName;
            var propertyValue = action.Value;
            var cards = action.Cards;

            for (var cardNumber = 0; cardNumber < CardCount; cardNumber++)
                NextPlayer[cardNumber].CheckPropertyValue(propertyName, propertyValue, cards.Contains(cardNumber));
        }
    }

    public class GameResult
    {
        public int Turn { get; }
        public int Cards { get; }
        public int WithRisk { get; }
        public GameOverException Cause { get; }

        public GameResult(int turn, int cards, int withRisk, GameOverException cause)
        {
            Turn = turn;
            Cards = cards;
            WithRisk = withRisk;
            Cause = cause;
        }

        public override string ToString()
        {
            return $"Turn: {Turn}, cards: {Cards}, with risk: {WithRisk}";
        }
    }

    public class Player
    {
        private List<Card> Cards { get; } = new List<Card>();

        public void Reset()
        {
            Cards.Clear();
        }

        public Card GetCard(int cardNumber)
        {
            return Cards[cardNumber];
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public Card RemoveCard(int number)
        {
            var card = Cards[number];
            Cards.RemoveAt(number);
            return card;
        }

        public string CardsAsString => Cards.Join();

        public Card this[int cardNumber] => GetCard(cardNumber);
    }

    public class Deck : IEnumerable<Card>
    {
        private Queue<Card> Cards { get; }

        public Deck()
        {
            Cards = new Queue<Card>();
        }

        public Deck(IEnumerable<Card> cards) : this()
        {
            Fill(cards);
        }

        public int Size => Cards.Count;
        public bool IsEmpty => Size == 0;

        public Card PollCard()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Deck is empty");
            return Cards.Dequeue();
        }

        public void Reset()
        {
            Cards.Clear();
        }

        public void Fill(IEnumerable<Card> cards)
        {
            Reset();
            foreach (var card in cards)
                Cards.Enqueue(card);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string CardsAsString => this.Join();

        public override string ToString()
        {
            return $"Deck: {CardsAsString}";
        }
    }

    public class Card
    {
        public Color Color { get; }
        public int Rank { get; }


        private Dictionary<Color, bool?> KnownColors { get; }
        private Dictionary<int, bool?> KnownRanks { get; }
        public static int RankCount => 5;


        private Card(Color color, int rank)
        {
            Color = color;
            Rank = rank;

            KnownColors = new Dictionary<Color, bool?>();
            KnownRanks = new Dictionary<int, bool?>();
            InitKnownColors();
            InitKnownRanks();
        }

        private void InitKnownColors()
        {
            KnownColors[Color.Red] = null;
            KnownColors[Color.Green] = null;
            KnownColors[Color.Blue] = null;
            KnownColors[Color.White] = null;
            KnownColors[Color.Yellow] = null;
        }

        private void InitKnownRanks()
        {
            for (var i = 1; i <= RankCount; i++)
                KnownRanks[i] = null;
        }

        public static Card Parse(string s)
        {
            var color = ParseColor(s[0]);
            var rank = s[1] - '0';
            return new Card(color, rank);
        }

        private static Color ParseColor(char c)
        {
            switch (c)
            {
                case 'R':
                    return Color.Red;
                case 'G':
                    return Color.Green;
                case 'B':
                    return Color.Blue;
                case 'W':
                    return Color.White;
                case 'Y':
                    return Color.Yellow;
                default:
                    throw new ArgumentException("Color is wrong");
            }
        }

        public void CheckPropertyValue(string propertyName, object propertyValue, bool shouldBeTrue)
        {
            var currentValue = GetPropertyValue(propertyName);
            if (currentValue.Equals(propertyValue) != shouldBeTrue)
                throw new GameOverException($"Lie! Wrong {propertyName}!");

            if (propertyValue is Color)
                KnownColors[(Color) propertyValue] = shouldBeTrue;
            else KnownRanks[(int) propertyValue] = shouldBeTrue;
        }

        public object GetPropertyValue(string propertyName)
        {
            return GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public IEnumerable<Color> AvailableColors => GetAvailablePropertyValues(KnownColors);

        public IEnumerable<int> AvailableRanks => GetAvailablePropertyValues(KnownRanks);

        private static IEnumerable<T> GetAvailablePropertyValues<T>(Dictionary<T, bool?> valuesHolder)
        {
            RecalculateProperty(valuesHolder);
            return valuesHolder
                .Where(keyValue => keyValue.Value.GetValueOrDefault(true))
                .Select(keyValue => keyValue.Key);
        }

        private static void RecalculateProperty<T>(Dictionary<T, bool?> valuesHolder)
        {
            if (valuesHolder.All(kv => kv.Value.HasValue)) return;

            if (valuesHolder.ContainsValue(true))
            {
                var rightKey = valuesHolder.First(keyValue => keyValue.Value == true).Key;
                foreach (var key in valuesHolder.Keys.ToArray())
                    valuesHolder[key] = key.Equals(rightKey);
                return;
            }

            if (valuesHolder.Count(prop => prop.Value == false) == valuesHolder.Count - 1)
                valuesHolder[valuesHolder.First(kv => !kv.Value.HasValue).Key] = true;
        }

        public override string ToString()
        {
            return $"{Color.Name[0]}{Rank}";
        }
    }

    public abstract class GameAction
    {
        public static GameAction Parse(string s)
        {
            if (s.StartsWith("Start new game"))
                return StartNewGameAction.Parse(s);
            if (s.StartsWith("Play card"))
                return PlayCardAction.Parse(s);
            if (s.StartsWith("Drop card"))
                return DropCardAction.Parse(s);
            if (s.StartsWith("Tell"))
                return TellCardPropertyAction.Parse(s);
            return null;
        }
    }

    public class StartNewGameAction : GameAction
    {
        private static readonly Regex Matcher = new Regex(@"Start new game with deck (?<Cards>.+)");
        private readonly Deck _deck;
        public Deck Deck => new Deck(_deck);

        private StartNewGameAction(IEnumerable<Card> cards)
        {
            _deck = new Deck(cards);
        }

        public new static StartNewGameAction Parse(string s)
        {
            return new StartNewGameAction(
                Matcher.Match(s).Groups["Cards"].Value
                    .Split()
                    .Select(Card.Parse));
        }

        public override string ToString()
        {
            return $"Start new game: Deck: {_deck.Join()}";
        }
    }

    public class PlayCardAction : GameAction
    {
        private static readonly Regex Matcher = new Regex(@"Play card (?<CardNumber>\d)");
        public int CardNumber { get; }

        private PlayCardAction(int cardNumber)
        {
            CardNumber = cardNumber;
        }

        public new static PlayCardAction Parse(string s)
        {
            return new PlayCardAction(int.Parse(Matcher.Match(s).Groups["CardNumber"].Value));
        }

        public override string ToString()
        {
            return $"Play card: CardNumber: {CardNumber}";
        }
    }

    public class DropCardAction : GameAction
    {
        private static readonly Regex Matcher = new Regex(@"Drop card (?<CardNumber>\d+)");

        public int CardNumber { get; }

        private DropCardAction(int cardNumber)
        {
            CardNumber = cardNumber;
        }

        public new static DropCardAction Parse(string s)
        {
            return new DropCardAction(int.Parse(Matcher.Match(s).Groups["CardNumber"].Value));
        }

        public override string ToString()
        {
            return $"Drop card: CardNumber: {CardNumber}";
        }
    }

    public class TellCardPropertyAction : GameAction
    {
        private static readonly Regex Matcher =
            new Regex(@"Tell (?<PropertyName>\w+) (?<Value>\w+) for cards (?<Cards>.+)");

        public string PropertyName { get; }
        public object Value { get; }
        public IReadOnlyList<int> Cards { get; }

        private TellCardPropertyAction(string propertyName, object value, IEnumerable<int> cards)
        {
            PropertyName = propertyName;
            Value = value;
            Cards = cards.ToArray();
        }

        public new static TellCardPropertyAction Parse(string s)
        {
            var matches = Matcher.Match(s).Groups;
            var propertyName = matches["PropertyName"].ToString();
            var value = ParseProperty(propertyName, matches["Value"].ToString());
            var cards = matches["Cards"].ToString().Split().Select(int.Parse);

            return new TellCardPropertyAction(propertyName, value, cards);
        }

        private static object ParseProperty(string name, string value)
        {
            switch (name)
            {
                case "color":
                    return Color.FromName(value);
                case "rank":
                    return int.Parse(value);
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return $"Tell {PropertyName}: Value: {Value}, Cards: {Cards.Join()}";
        }
    }

    public class GameOverException : Exception
    {
        public GameOverException(string s) : base(s)
        {
        }
    }

    public static class EnumerableExtensions
    {
        public static string Join<T>(this IEnumerable<T> sequence, string separator = " ")
        {
            return string.Join(separator, sequence);
        }
    }
}
