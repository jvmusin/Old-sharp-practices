namespace linq_practice
{
    public class User
    {
        public string Id { get; }

        public User(string id)
        {
            Id = id;
        }

        protected bool Equals(User other)
        {
            return Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Id: {Id}";
        }
    }
}
