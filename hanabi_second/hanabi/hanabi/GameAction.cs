namespace hanabi
{
    public abstract class GameAction
    {
        public static GameAction Parse(string s)
        {
            if (s.StartsWith("Start new game")) return StartNewGameAction.Parse(s);
            if (s.StartsWith("Play card"))      return PlayCardAction.Parse(s);
            if (s.StartsWith("Drop card"))      return DropCardAction.Parse(s);
            if (s.StartsWith("Tell"))           return TellCardPropertyAction.Parse(s);
            return null;
        }
    }
}
