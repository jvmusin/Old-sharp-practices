using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace hanabi
{
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
}
