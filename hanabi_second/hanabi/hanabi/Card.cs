using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace hanabi
{
    public class Card
    {
        public Color Color { get; }
        public int Rank { get; }
        
        //  Contains 'true' if we already know about color/rank, 'false' if we know that this card has other color/rank, 'null' if we know nothing
        private Dictionary<Color, bool?> KnownColors { get; }
        private Dictionary<int, bool?> KnownRanks { get; }

        private const int MinRank = 1;
        private const int MaxRank = 5;
        
        private Card(Color color, int rank)
        {
            Color = color;
            Rank = rank;

            KnownColors = new Dictionary<Color, bool?>();
            KnownRanks = new Dictionary<int, bool?>();
            InitColors();
            InitRanks();
        }

        private void InitColors()
        {
            KnownColors[Color.Red]    = null;
            KnownColors[Color.Green]  = null;
            KnownColors[Color.Blue]   = null;
            KnownColors[Color.White]  = null;
            KnownColors[Color.Yellow] = null;
        }

        private void InitRanks()
        {
            for (var rank = MinRank; rank <= MaxRank; rank++)
                KnownRanks[rank] = null;
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
                case 'R': return Color.Red;
                case 'G': return Color.Green;
                case 'B': return Color.Blue;
                case 'W': return Color.White;
                case 'Y': return Color.Yellow;
                default:  throw new ArgumentException("Color is wrong");
            }
        }

        //  Checks whether current card has necessary color/rank (if 'expects' is false, checks the same, but inversed)
        public void CheckPropertyValue(string propertyName, object propertyValue, bool expects)
        {
            var currentValue = GetPropertyValue(propertyName);
            if (currentValue.Equals(propertyValue) != expects)
                throw new GameOverException($"Lie! Wrong {propertyName}!");

            if (propertyValue is Color)
                KnownColors[(Color) propertyValue] = expects;
            else KnownRanks[(int) propertyValue] = expects;
        }

        public object GetPropertyValue(string propertyName)
        {
            return GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public IEnumerable<Color> PossibleColors => GetPossiblePropertyValues(KnownColors);
        public IEnumerable<int> PossibleRanks => GetPossiblePropertyValues(KnownRanks);

        private static IEnumerable<T> GetPossiblePropertyValues<T>(Dictionary<T, bool?> valuesHolder)
        {
            RecalculateProperty(valuesHolder);
            return valuesHolder
                .Where(propertyAndValue => propertyAndValue.Value != false)
                .Select(propertyAndValue => propertyAndValue.Key);
        }

        private static void RecalculateProperty<T>(Dictionary<T, bool?> valuesHolder)
        {
            //  If there are no unknown variants just go out
            if (valuesHolder.All(propertyAndValue => propertyAndValue.Value.HasValue))
                return;

            //  If there are 'true' value, we should note that other variants are definitely 'false'
            if (valuesHolder.ContainsValue(true))
            {
                var rightPropertyValue = valuesHolder
                    .First(propertyAndValue => propertyAndValue.Value == true).Key;
                foreach (var key in valuesHolder.Keys.ToArray())
                    valuesHolder[key] = key.Equals(rightPropertyValue);
                return;
            }

            //  If only one possible variant remained, we should mark it with 'true' value
            if (valuesHolder.Count(propertyAndValue => propertyAndValue.Value == false) == valuesHolder.Count - 1)
            {
                var rightPropertyValue = valuesHolder
                    .First(propertyAndValue => !propertyAndValue.Value.HasValue).Key;
                valuesHolder[rightPropertyValue] = true;
            }
        }

        public override string ToString()
        {
            return $"{Color.Name[0]}{Rank}";
        }
    }
}
