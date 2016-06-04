using System.Text.RegularExpressions;

namespace hanabi
{
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
}
