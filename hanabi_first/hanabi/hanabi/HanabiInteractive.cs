using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hanabi
{
    public class HanabiInteractive
    {
        private const int CardCount = 5;
        public bool Debug { get; set; }

        private int Turn { get; set; }
        private int Score { get; set; }
        private bool Finished { get; set; }

        private List<Card> FirstPlayer { get; }
        private List<Card> SecondPlayer { get; }
        private bool FirstPlayerTurns { get; set; }
        private List<Card> CurrentPlayer => FirstPlayerTurns ? FirstPlayer : SecondPlayer;
        private List<Card> NextPlayer => FirstPlayerTurns ? SecondPlayer : FirstPlayer;

        private Queue<Card> Deck { get; }
        private Dictionary<Color, int> Table { get; }
        private Dictionary<Type, Action<GameAction>> Actions { get; }

        public HanabiInteractive()
        {
            FirstPlayer = new List<Card>();
            SecondPlayer = new List<Card>();
            Deck = new Queue<Card>();
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

        private void ResetGame()
        {
            Turn = 0;
            Score = 0;
            Finished = false;
            FirstPlayerTurns = true;
            ResetPlayers();
            ResetTable();
        }

        private void InitDeck(IEnumerable<Card> deck)
        {
            Deck.Clear();
            foreach (var card in deck)
                Deck.Enqueue(card);
        }

        private void InitPlayer(ICollection<Card> player)
        {
            for (var i = 0; i < CardCount; i++)
                player.Add(NextCard());
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

        private Card NextCard()
        {
            return Deck.Dequeue();
        }

        private void CheckDeck()
        {
            if (Deck.Count == 0)
                throw new GameOverException("Deck is empty!");
        }

        public GameResult MakeTurn(string command)
        {
            if (Finished && !command.StartsWith("Start new game"))
                return null;

            GameResult result = null;
            try
            {
                Turn++;
                var action = GameAction.Parse(command);
                if (Turn > 1 && !(action is StartNewGameAction))
                    SwapPlayers();
                Actions[action.GetType()](action);
                CheckDeck();
            }
            catch (GameOverException e)
            {
                Finished = true;
                result = new GameResult(Turn, Score, e);
            }

            if (Debug)
                Console.WriteLine(PrepareDebugInfo());
            return result;
        }

        private string PrepareDebugInfo()
        {
            const int labelWidth = -20;
            const int elementWidth = -5;
            Func<IEnumerable<Card>, string> join =
                seq => seq.Aggregate("", (str, obj) => str + $"{obj,elementWidth}");

            var main = $"Turn: {Turn}, Score: {Score}, Finished: {Finished}";

            var firstPlayerLabel = "First Player" + (FirstPlayerTurns ? "(!)" : "");
            var secondPlayerLabel = "Second Player" + (FirstPlayerTurns ? "" : "(!)");

            var currentPlayer = $"{firstPlayerLabel + ": ",labelWidth}" + join(CurrentPlayer);
            var nextPlayer = $"{secondPlayerLabel + ": ",labelWidth}" + join(NextPlayer);
            var deck = $"{"Deck: ",labelWidth}" + join(Deck);

            var table = $"{"Table: ",labelWidth}";
            table += string.Join(" ", Table.Select(cards => $"[{cards.Key.Name} {cards.Value}]"));

            return string.Join("\n", main, currentPlayer, nextPlayer, table, deck);
        }

        private void StartNewGame(StartNewGameAction action)
        {
            ResetGame();
            InitDeck(action.Deck);
            InitPlayers();
        }

        private void PlayCard(PlayCardAction action)
        {
            var card = CurrentPlayer[action.CardNumber];
            CurrentPlayer.RemoveAt(action.CardNumber);
            PutCard(card);
            CurrentPlayer.Add(NextCard());
        }

        private void PutCard(Card card)
        {
            if (++Table[card.Color] != card.Rank)
                throw new GameOverException("Wrong rank!");
            Score++;
        }

        private void DropCard(DropCardAction action)
        {
            CurrentPlayer.RemoveAt(action.CardNumber);
            CurrentPlayer.Add(NextCard());
        }

        private void TellCardProperty(TellCardPropertyAction action)
        {
            var propertyName = action.PropertyName;
            var propertyValue = action.Value;
            var cards = action.Cards;

            foreach (var card in cards)
                NextPlayer[card].CheckProperty(propertyValue);

            if (NextPlayer.Count(card => card.GetProperty(propertyName).Equals(propertyValue)) != cards.Count)
                throw new GameOverException($"Lie! Not full info about {propertyName}!");
        }
    }
}
