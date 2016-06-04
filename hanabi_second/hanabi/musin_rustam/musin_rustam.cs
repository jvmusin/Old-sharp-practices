using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace musin_rustam
{
    public class Musin_Rustam
    {
        public static void Main(string[] args)
        {
            var hanabi = new HanabiInteractive();
            var gameResults = ReadLines().Select(hanabi.MakeTurn).Where(result => result != null);
            foreach (var result in gameResults)
                Console.WriteLine(result);
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
        private const int CardsCount = 5;

        private int Turn { get; set; }
        private int Score { get; set; }
        private int WithRisk { get; set; }
        private bool Finished { get; set; }

        private IList<Card> FirstPlayer { get; }
        private IList<Card> SecondPlayer { get; }
        private bool FirstPlayerTurns => Turn == 0 || Turn % 2 == 1;
        private IList<Card> CurrentPlayer => FirstPlayerTurns ? FirstPlayer : SecondPlayer;
        private IList<Card> NextPlayer    => FirstPlayerTurns ? SecondPlayer : FirstPlayer;

        private Deck Deck { get; }
        private IDictionary<Color, int> Table { get; }
        private IDictionary<Type, Action<GameAction>> Actions { get; }


        public HanabiInteractive()
        {
            Finished = true;
            FirstPlayer = new List<Card>();
            SecondPlayer = new List<Card>();
            Deck = new Deck();
            Table = new Dictionary<Color, int>();
            Actions = new Dictionary<Type, Action<GameAction>>();
            InitActions();
        }

        private void InitActions()
        {
            Actions[typeof(StartNewGameAction)] = action => StartNewGame((StartNewGameAction)action);
            Actions[typeof(PlayCardAction)] = action => PlayCard(((PlayCardAction)action).CardNumber);
            Actions[typeof(DropCardAction)] = action => DropCard(((DropCardAction)action).CardNumber);
            Actions[typeof(TellCardPropertyAction)] = action => TellCardProperty((TellCardPropertyAction)action);
        }

        private void ResetPlayers()
        {
            FirstPlayer.Clear();
            SecondPlayer.Clear();
        }

        private void ResetTable()
        {
            Table[Color.Red] = 0;
            Table[Color.Green] = 0;
            Table[Color.Blue] = 0;
            Table[Color.White] = 0;
            Table[Color.Yellow] = 0;
        }

        private void RestartGame()
        {
            Turn = 0;
            Score = 0;
            WithRisk = 0;
            Finished = false;
            ResetPlayers();
            ResetTable();
        }

        private void InitDeck(IEnumerable<Card> cards)
        {
            Deck.Fill(cards);
        }

        private void InitPlayer(ICollection<Card> player)
        {
            for (var i = 0; i < CardsCount; i++)
                player.Add(Deck.PollCard());
        }

        private void InitPlayers()
        {
            InitPlayer(FirstPlayer);
            InitPlayer(SecondPlayer);
        }

        private void TryFinishGame()
        {
            if (Deck.Empty)
                throw new GameOverException("Deck is empty!");
            if (Score == 25)
                throw new GameOverException("Table is filled!");
        }

        public GameResult MakeTurn(string command)
        {
            GameResult result = null;
            try
            {
                var action = GameAction.Parse(command);
                if (Finished && !(action is StartNewGameAction))
                    return null;
                Turn++;
                Actions[action.GetType()](action);
                TryFinishGame();
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
            RestartGame();
            InitDeck(action.Deck);
            InitPlayers();
        }

        private void PlayCard(int cardNumber)
        {
            var card = CurrentPlayer[cardNumber];
            var risky = IsRisky(card);
            PutCard(card);
            if (risky) WithRisk++;
            DropCard(cardNumber);
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
                from color in card.PossibleColors
                from rank in card.PossibleRanks
                where IsRisky(color, rank)
                select true).Any();
        }

        private bool IsRisky(Color color, int rank)
        {
            return Table[color] + 1 != rank;
        }

        private void DropCard(int cardNumber)
        {
            CurrentPlayer.RemoveAt(cardNumber);
            CurrentPlayer.Add(Deck.PollCard());
        }

        private void TellCardProperty(TellCardPropertyAction action)
        {
            var propertyName = action.PropertyName;
            var propertyValue = action.Value;
            var cards = action.Cards;

            for (var cardNumber = 0; cardNumber < CardsCount; cardNumber++)
            {
                var queryContainsCurrentCard = cards.Contains(cardNumber);
                NextPlayer[cardNumber]
                    .CheckPropertyValue(propertyName, propertyValue, queryContainsCurrentCard);
            }
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

    public class Deck : IEnumerable<Card>
    {
        private Queue<Card> Cards { get; }
        public int Size => Cards.Count;
        public bool Empty => Size == 0;

        public Deck()
        {
            Cards = new Queue<Card>();
        }

        public Deck(IEnumerable<Card> cards) : this()
        {
            Fill(cards);
        }

        public Card PollCard()
        {
            return Cards.Dequeue();
        }

        public void Fill(IEnumerable<Card> cards)
        {
            Cards.Clear();
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

        public override string ToString()
        {
            return $"Deck: {this.Join()}";
        }
    }

    public class Card
    {
        public Color Color { get; }
        public int Rank { get; }

        //  Contains 'true' if we already know about color/rank, 'false' if we know that this card has other color/rank, 'null' if we know nothing
        private Dictionary<Color, bool?> KnownColors { get; }
        private Dictionary<int, bool?> KnownRanks { get; }

        private const int MinRank = 1;
        private const int MaxRank = 5;

        private Card(Color color, int rank)
        {
            Color = color;
            Rank = rank;

            KnownColors = new Dictionary<Color, bool?>();
            KnownRanks = new Dictionary<int, bool?>();
            InitColors();
            InitRanks();
        }

        private void InitColors()
        {
            KnownColors[Color.Red] = null;
            KnownColors[Color.Green] = null;
            KnownColors[Color.Blue] = null;
            KnownColors[Color.White] = null;
            KnownColors[Color.Yellow] = null;
        }

        private void InitRanks()
        {
            for (var rank = MinRank; rank <= MaxRank; rank++)
                KnownRanks[rank] = null;
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
                case 'R': return Color.Red;
                case 'G': return Color.Green;
                case 'B': return Color.Blue;
                case 'W': return Color.White;
                case 'Y': return Color.Yellow;
                default: throw new ArgumentException("Color is wrong");
            }
        }

        //  Checks whether current card has necessary color/rank (if 'expects' is false, checks the same, but inversed)
        public void CheckPropertyValue(string propertyName, object propertyValue, bool expects)
        {
            var currentValue = GetPropertyValue(propertyName);
            if (currentValue.Equals(propertyValue) != expects)
                throw new GameOverException($"Lie! Wrong {propertyName}!");

            if (propertyValue is Color)
                KnownColors[(Color)propertyValue] = expects;
            else KnownRanks[(int)propertyValue] = expects;
        }

        public object GetPropertyValue(string propertyName)
        {
            return GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public IEnumerable<Color> PossibleColors => GetPossiblePropertyValues(KnownColors);
        public IEnumerable<int> PossibleRanks => GetPossiblePropertyValues(KnownRanks);

        private static IEnumerable<T> GetPossiblePropertyValues<T>(Dictionary<T, bool?> valuesHolder)
        {
            RecalculateProperty(valuesHolder);
            return valuesHolder
                .Where(propertyAndValue => propertyAndValue.Value != false)
                .Select(propertyAndValue => propertyAndValue.Key);
        }

        private static void RecalculateProperty<T>(Dictionary<T, bool?> valuesHolder)
        {
            //  If there are no unknown variants just go out
            if (valuesHolder.All(propertyAndValue => propertyAndValue.Value.HasValue))
                return;

            //  If there are 'true' value, we should note that other variants are definitely 'false'
            if (valuesHolder.ContainsValue(true))
            {
                var rightPropertyValue = valuesHolder
                    .First(propertyAndValue => propertyAndValue.Value == true).Key;
                foreach (var key in valuesHolder.Keys.ToArray())
                    valuesHolder[key] = key.Equals(rightPropertyValue);
                return;
            }

            //  If only one possible variant remained, we should mark it with 'true' value
            if (valuesHolder.Count(propertyAndValue => propertyAndValue.Value == false) == valuesHolder.Count - 1)
            {
                var rightPropertyValue = valuesHolder
                    .First(propertyAndValue => !propertyAndValue.Value.HasValue).Key;
                valuesHolder[rightPropertyValue] = true;
            }
        }

        public override string ToString()
        {
            return $"{Color.Name[0]}{Rank}";
        }
    }

    #region Game action classes

    public abstract class GameAction
    {
        public static GameAction Parse(string s)
        {
            if (s.StartsWith("Start new game")) return StartNewGameAction.Parse(s);
            if (s.StartsWith("Play card"))      return PlayCardAction.Parse(s);
            if (s.StartsWith("Drop card"))      return DropCardAction.Parse(s);
            if (s.StartsWith("Tell"))           return TellCardPropertyAction.Parse(s);
            return null;
        }
    }

    public class StartNewGameAction : GameAction
    {
        private static readonly Regex Parser = new Regex(@"Start new game with deck (?<Cards>.+)");
        public Deck Deck { get; }

        private StartNewGameAction(IEnumerable<Card> cards)
        {
            Deck = new Deck(cards);
        }

        public new static StartNewGameAction Parse(string s)
        {
            return new StartNewGameAction(
                Parser.Match(s).Groups["Cards"].Value
                    .Split()
                    .Select(Card.Parse));
        }

        public override string ToString()
        {
            return $"Start new game: {Deck}";
        }
    }

    public class PlayCardAction : GameAction
    {
        private static readonly Regex Parser = new Regex(@"Play card (?<CardNumber>\d)");
        public int CardNumber { get; }

        private PlayCardAction(int cardNumber)
        {
            CardNumber = cardNumber;
        }

        public new static PlayCardAction Parse(string s)
        {
            var cardNumber = int.Parse(Parser.Match(s).Groups["CardNumber"].Value);
            return new PlayCardAction(cardNumber);
        }

        public override string ToString()
        {
            return $"Play card: CardNumber: {CardNumber}";
        }
    }

    public class DropCardAction : GameAction
    {
        private static readonly Regex Parser = new Regex(@"Drop card (?<CardNumber>\d)");

        public int CardNumber { get; }

        private DropCardAction(int cardNumber)
        {
            CardNumber = cardNumber;
        }

        public new static DropCardAction Parse(string s)
        {
            var cardNumber = int.Parse(Parser.Match(s).Groups["CardNumber"].Value);
            return new DropCardAction(cardNumber);
        }

        public override string ToString()
        {
            return $"Drop card: CardNumber: {CardNumber}";
        }
    }

    public class TellCardPropertyAction : GameAction
    {
        private static readonly Regex Parser =
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
            var groups = Parser.Match(s).Groups;
            var propertyName = groups["PropertyName"].Value;
            var value = ParseProperty(propertyName, groups["Value"].Value);
            var cards = groups["Cards"].Value.Split().Select(int.Parse);

            return new TellCardPropertyAction(propertyName, value, cards);
        }

        private static object ParseProperty(string name, string value)
        {
            switch (name)
            {
                case "color": return Color.FromName(value);
                case "rank":  return int.Parse(value);
                default:      return null;
            }
        }

        public override string ToString()
        {
            return $"Tell {PropertyName}: Value: {Value}, Cards: {Cards.Join()}";
        }
    }

    #endregion

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
