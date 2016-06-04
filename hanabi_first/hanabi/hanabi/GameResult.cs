namespace hanabi
{
    public class GameResult
    {
        public int Turn { get; }
        public int Cards { get; }
        public GameOverException Cause { get; }

        public GameResult(int turn, int cards, GameOverException cause)
        {
            Turn = turn;
            Cards = cards;
            Cause = cause;
        }

        public override string ToString()
        {
            return $"Turn: {Turn}, cards: {Cards}, with risk: 0";
        }
    }
}
