using System.Collections.Generic;
using System.Linq;
using static helpers.Helpers;

namespace task1494_monobilliards
{
    public class Solver
    {
        public static bool IsCheater(IReadOnlyList<int> takingOrder)
        {
            var ballCount = takingOrder.Count;
            var hole = new Stack<int>();
            var lastUsedBallNumber = 0;

            foreach (var nextTakenBall in takingOrder)
            {
                while (hole.Count == 0 || hole.Peek() != nextTakenBall)
                {
                    if (lastUsedBallNumber == ballCount)
                        return true;
                    hole.Push(++lastUsedBallNumber);
                }
                hole.Pop();
            }
            return false;
        }

        public static void Main()
        {
            var ballCount = ReadInt();
            var takingOrder = Enumerable.Range(0, ballCount)
                .Select(_ => ReadInt())
                .ToArray();
            var isCheater = IsCheater(takingOrder);

            WriteLine(isCheater ? "Cheater" : "Not a proof");
        }
    }
}
