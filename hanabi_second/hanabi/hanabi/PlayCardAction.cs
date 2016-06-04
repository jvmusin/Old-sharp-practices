using System.Text.RegularExpressions;

namespace hanabi
{
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
}
