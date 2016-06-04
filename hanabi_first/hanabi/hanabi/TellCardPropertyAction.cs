using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace hanabi
{
    public class TellCardPropertyAction : GameAction
    {
        private static readonly Regex Mather =
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
            var matches = Mather.Match(s).Groups;
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
            return $"Tell {PropertyName}: Value: {Value}, Cards: {string.Join(" ", Cards)}";
        }
    }
}
