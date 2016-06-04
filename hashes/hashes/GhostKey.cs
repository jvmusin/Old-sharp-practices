using System;

namespace hashes
{
	public class GhostKey
	{
		public string Name { get; }
	    private int hashKiller;

		public GhostKey(string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			Name = name;
		}

		public void DoSomething()
		{
		    hashKiller++;
		}

	    public override bool Equals(object obj)
	    {
	        var other = obj as GhostKey;
            return Equals(Name, other?.Name);
	    }

	    public override int GetHashCode()
	    {
	        return Name.GetHashCode() + hashKiller;
	    }
	}
}