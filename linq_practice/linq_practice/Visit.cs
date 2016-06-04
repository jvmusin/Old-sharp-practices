using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace linq_practice
{
    public class Visit
    {
        public User User { get; }
        public Slide Slide { get; }
        public DateTime Date { get; }

        public Visit(User user, Slide slide, DateTime date)
        {
            User = user;
            Slide = slide;
            Date = date;
        }

        private static readonly Regex Parser = new Regex(@"(?<UserId>.+);(?<SlideId>.+);(?<Date>.+);(?<Time>.+)");
        public static Visit Parse(string s, IDictionary<string, Slide> slides, Dictionary<string, User> users)
        {
            var result = Parser.Match(s).Groups;

            var userId = result["UserId"].ToString();
            if (!users.ContainsKey(userId))
                users[userId] = new User(userId);

            var user = users[userId];
            var slide = slides[result["SlideId"].Value];
            var date = DateTime.Parse(result["Date"] + "T" + result["Time"]);

            return new Visit(user, slide, date);
        }

        public static TimeSpan operator -(Visit v1, Visit v2)
        {
            return v1.Date - v2.Date;
        }

        public override string ToString()
        {
            return $"UserId: {User}, Slide: {{{Slide}}}, Date: {Date}";
        }

        public bool Equals(Visit other)
        {
            return Equals(User, other.User) &&
                   Equals(Slide, other.Slide) &&
                   Equals(Date, other.Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Visit) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = User.GetHashCode();
                hashCode = (hashCode*397) ^ Slide.GetHashCode();
                hashCode = (hashCode*397) ^ Date.GetHashCode();
                return hashCode;
            }
        }
    }
}
