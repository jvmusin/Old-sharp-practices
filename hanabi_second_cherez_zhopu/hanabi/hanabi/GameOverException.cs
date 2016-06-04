using System;

namespace hanabi
{
    public class GameOverException : Exception
    {
        public GameOverException(string s) : base(s)
        {
        }
    }
}
