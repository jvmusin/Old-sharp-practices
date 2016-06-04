using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace hanabi
{
    public class HanabiInteractive
    {
        private const int CardsCount = 5;
        public bool Debug { get; set; }

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
            Actions[typeof (StartNewGameAction)]     = action => StartNewGame((StartNewGameAction) action);
            Actions[typeof (PlayCardAction)]         = action => PlayCard(((PlayCardAction) action).CardNumber);
            Actions[typeof (DropCardAction)]         = action => DropCard(((DropCardAction) action).CardNumber);
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

            if (Debug)
                Console.WriteLine(PrepareDebugInfo());
            return result;
        }

        private string PrepareDebugInfo()
        {
            const int labelWidth = -20;

            var main = $"Turn: {Turn}, Score: {Score}, With risk: {WithRisk}, Finished: {Finished}";

            var firstPlayerLabel = $"First Player{(FirstPlayerTurns ? "(!)" : "")}";
            var secondPlayerLabel = $"Second Player{(FirstPlayerTurns ? "" : "(!)")}";

            var firstPlayer = $"{firstPlayerLabel + ": ",labelWidth}{FirstPlayer.Join()}";
            var secondPlayer = $"{secondPlayerLabel + ": ",labelWidth}{SecondPlayer.Join()}";
            var deck = $"{"Deck: ",labelWidth}{Deck}";

            var table = $"{"Table: ",labelWidth}" +
                        $"{string.Join(" ", Table.Select(colorAndRank => $"[{colorAndRank.Key.Name} {colorAndRank.Value}]"))}";

            return string.Join("\n", main, firstPlayer, secondPlayer, table, deck);
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
}
