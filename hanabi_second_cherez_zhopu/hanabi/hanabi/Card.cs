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


        private Dictionary<Color, bool?> KnownColors { get; }
        private Dictionary<int, bool?> KnownRanks { get; }
        public const int RankCount = 5;

        private Card(Color color, int rank)
        {
            Color = color;
            Rank = rank;

            KnownColors = new Dictionary<Color, bool?>();
            KnownRanks = new Dictionary<int, bool?>();
            InitKnownColors();
            InitKnownRanks();
        }

        private void InitKnownColors()
        {
            KnownColors[Color.Red] = null;
            KnownColors[Color.Green] = null;
            KnownColors[Color.Blue] = null;
            KnownColors[Color.White] = null;
            KnownColors[Color.Yellow] = null;
        }

        private void InitKnownRanks()
        {
            for (var i = 1; i <= RankCount; i++)
                KnownRanks[i] = null;
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

        public void CheckProperty(string propertyName, object propertyValue, bool shouldBeTrue)
        {
            var currentValue = GetProperty(propertyName);
            if (currentValue.Equals(propertyValue) != shouldBeTrue)
                throw new GameOverException($"Lie! Wrong {propertyName}");

            if (propertyValue is Color)
                KnownColors[(Color) propertyValue] = shouldBeTrue;
            else KnownRanks[(int) propertyValue] = shouldBeTrue;
        }

        public object GetProperty(string name)
        {
            return GetType()
                .GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                .GetValue(this);
        }

        public IEnumerable<Color> GetAvailableColors()
        {
            RecalculateProperty(KnownColors);
            return KnownColors
                .Where(color => color.Value.GetValueOrDefault(true))
                .Select(color => color.Key);
        }

        public IEnumerable<int> GetAvailableRanks()
        {
            RecalculateProperty(KnownRanks);
            return KnownRanks
                .Where(rank => !rank.Value.HasValue || rank.Value != false)
                .Select(rank => rank.Key);
        }

        private static void RecalculateProperty<T>(Dictionary<T, bool?> holder)
        {
            if (holder.Count(kv => kv.Value.HasValue) == holder.Count) return;

            if (holder.Count(prop => prop.Value == false) == 4)
                holder[holder.First(kv => !kv.Value.HasValue).Key] = true;
            if (holder.ContainsValue(true))
            {
                var rightKey = holder.First(keyValue => keyValue.Value == true).Key;
                foreach (var key in holder.Keys.ToArray())
                    holder[key] = key.Equals(rightKey);
            }
        }

        public override string ToString()
        {
            return $"{Color.Name[0]}{Rank}";
        }
    }
}
