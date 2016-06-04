using System;
using System.Collections.Generic;
using System.Linq;

namespace helpers
{
    public static class Helpers
    {
        #region Readers

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static IEnumerable<string> ReadTokens()
        {
            return ReadLine().Split();
        }

        public static IEnumerable<int> ReadInts()
        {
            return ReadTokens().Select(int.Parse);
        }

        public static int ReadInt()
        {
            return ReadInts().First();
        }

        #endregion

        #region Writers

        public static void Write<T>(T elem)
        {
            Console.Write(elem);
        }

        public static void WriteLine()
        {
            Write('\n');
        }

        public static void WriteLine<T>(T elem)
        {
            Write(elem);
            WriteLine();
        }

        public static void Write<T>(IEnumerable<T> elements)
        {
            var s = elements as string;
            if (s != null)
            {
                Write(s);
                return;
            }

            var firstPrinted = false;
            foreach (var element in elements)
            {
                if (firstPrinted)
                    Write(' ');
                Write(element);
                firstPrinted = true;
            }
        }

        public static void WriteLine<T>(IEnumerable<T> elements)
        {
            Write(elements);
            WriteLine();
        }

        #endregion
    }

    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsLower(s[0]))
                return s;
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
