using System.Collections;
using System.Collections.Generic;

namespace hanabi
{
    public class Deck : IEnumerable<Card>
    {
        private Queue<Card> Cards { get; }
        public int Size => Cards.Count;
        public bool Empty => Size == 0;

        public Deck()
        {
            Cards = new Queue<Card>();
        }

        public Deck(IEnumerable<Card> cards) : this()
        {
            Fill(cards);
        }

        public Card PollCard()
        {
            return Cards.Dequeue();
        }

        public void Fill(IEnumerable<Card> cards)
        {
            Cards.Clear();
            foreach (var card in cards)
                Cards.Enqueue(card);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"Deck: {this.Join()}";
        }
    }
}
