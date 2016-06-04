using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace linq_practice
{
    public class Slide
    {
        public string Id { get; }
        public SlideType Type { get; }
        public string UnitTitle { get; }

        public Slide(string id, string type, string unitTitle)
        {
            Id = id;
            Type = SlideType.FromName(type);
            UnitTitle = unitTitle;
        }

        private static readonly Regex Parser = new Regex(@"(?<Id>\w+);(?<Type>\w+);(?<UnitTitle>.+)");

        public static Slide Parse(string s)
        {
            var result = Parser.Match(s).Groups;
            return new Slide(
                result["Id"].Value,
                result["Type"].Value,
                result["UnitTitle"].Value);
        }

        protected bool Equals(Slide other)
        {
            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Slide) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"SlideId: {Id}, SlideType: {Type}, UnitTitle: {UnitTitle}";
        }
    }

    public class SlideType
    {
        public string Name { get; }

        private SlideType(string name)
        {
            Name = name;
        }

        private static readonly Dictionary<string, SlideType> Types = new Dictionary<string, SlideType>();
        public static SlideType FromName(string name)
        {
            if (!Types.ContainsKey(name))
                Types[name] = new SlideType(name);
            return Types[name];
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
