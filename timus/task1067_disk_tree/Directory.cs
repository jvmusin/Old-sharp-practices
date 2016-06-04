using System;
using System.Collections.Generic;
using System.Linq;

namespace task1067_disk_tree
{
    public class Directory
    {
        public string Name { get; }
        public int Depth { get; }
        private IDictionary<string, Directory> Children { get; }

        public Directory(string name, Directory parent = null)
        {
            Name = name;
            Depth = parent?.Depth + 1 ?? -1;
            Children = new SortedDictionary<string, Directory>(StringComparer.Ordinal);
        }

        public Directory ContinueBy(string directoryName)
        {
            if (Children.ContainsKey(directoryName)) return Children[directoryName];
            return Children[directoryName] = new Directory(directoryName, this);
        }

        public IEnumerable<string> EnumerateTree()
        {
            if (Depth != -1)
                yield return ToString();

            foreach (var childName in Children.Values.SelectMany(child => child.EnumerateTree()))
                yield return childName;
        }

        public override string ToString()
        {
            return string.Format("{0," + (Depth + Name.Length) + "}", Name);
        }
    }
}
