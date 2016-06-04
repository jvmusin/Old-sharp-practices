using System;

namespace hashes
{
	public class GhostKey
	{
		public string Name { get; private set; }

		public GhostKey(string name)
		{
			if (name == null) throw new ArgumentNullException("name");
			Name = name;
		}

		public void DoSomething()
		{
		    Name += "1";
		}

	    public override bool Equals(object obj)
	    {
	        var other = obj as GhostKey;
	        return other != null && Name.Equals(other.Name);
	    }

	    public override int GetHashCode()
	    {
	        return Name.GetHashCode();
	    }
	}
}