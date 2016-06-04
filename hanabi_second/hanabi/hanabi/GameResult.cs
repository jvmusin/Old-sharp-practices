namespace hanabi
{
    public class GameResult
    {
        public int Turn { get; }
        public int Cards { get; }
        public int WithRisk { get; }
        public GameOverException Cause { get; }

        public GameResult(int turn, int cards, int withRisk, GameOverException cause)
        {
            Turn = turn;
            Cards = cards;
            WithRisk = withRisk;
            Cause = cause;
        }

        public override string ToString()
        {
            return $"Turn: {Turn}, cards: {Cards}, with risk: {WithRisk}";
        }
    }
}
