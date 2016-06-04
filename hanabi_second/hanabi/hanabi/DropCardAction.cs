using System.Text.RegularExpressions;

namespace hanabi
{
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
}
