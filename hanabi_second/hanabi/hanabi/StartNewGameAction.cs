using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace hanabi
{
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
}
