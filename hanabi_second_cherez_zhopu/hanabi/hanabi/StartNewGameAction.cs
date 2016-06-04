using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace hanabi
{
    public class StartNewGameAction : GameAction
    {
        private static readonly Regex Matcher = new Regex(@"Start new game with deck (?<Cards>.+)");
        private readonly Queue<Card> _deck;
        public Queue<Card> Deck => new Queue<Card>(_deck);

        private StartNewGameAction(IEnumerable<Card> cards)
        {
            _deck = new Queue<Card>(cards);
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
            return $"Start new game: Deck: {_deck}";
        }
    }
}
