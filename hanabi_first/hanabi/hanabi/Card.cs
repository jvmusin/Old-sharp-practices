using System;
using System.Drawing;
using System.Reflection;

namespace hanabi
{
    public class Card
    {
        public Color Color { get; }
        public int Rank { get; }


        private Card(Color color, int rank)
        {
            Color = color;
            Rank = rank;
        }

        public static Card Parse(string s)
        {
            var color = ParseColor(s[0]);
            var rank = s[1] - '0';
            return new Card(color, rank);
        }

        private static Color ParseColor(char c)
        {
            switch (c)
            {
                case 'R':
                    return Color.Red;
                case 'G':
                    return Color.Green;
                case 'B':
                    return Color.Blue;
                case 'W':
                    return Color.White;
                case 'Y':
                    return Color.Yellow;
                default:
                    throw new ArgumentException("Color is wrong");
            }
        }

        public void CheckProperty(object propertyValue)
        {
            var propertyName = propertyValue is Color ? "Color" : "Rank";
            var currentValue = GetType().GetProperty(propertyName).GetValue(this);
            if (!currentValue.Equals(propertyValue))
                throw new GameOverException($"Lie! Wrong {propertyName}");
        }

        public object GetProperty(string name)
        {
            return GetType()
                .GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public override string ToString()
        {
            return $"{Color.Name[0]}{Rank}";
        }
    }
}
