using System.Text.RegularExpressions;

namespace hanabi
{
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
}
