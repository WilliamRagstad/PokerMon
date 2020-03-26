using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerMonServer
{
    class Deck
    {
        private Card[] _cards;
        private int _currentCard;

        public Deck()
        {
            int capacity = 52;
            _cards = new Card[capacity];
            _currentCard = 0;

            // Generate deck
            int suit = 0;
            int rank = 0;
            for (int i = 0; i < capacity; i++)
            {
                _cards[i] = new Card((CRank)rank, (CSuit)suit);
                rank++;
                if (rank >= 13)
                {
                    rank = 0;
                    suit++;
                }
            }
        }

        public void Shuffle()
        {
            Random rnd = new Random();
           _cards = _cards.OrderBy(x => rnd.Next()).ToArray();
            _currentCard = 0;
        }

        public Card Draw()
        {
            if (_currentCard >= _cards.Length)
                throw new DeckOutOfCardsException("There are no more cards in the deck. Please shuffle.");
            return _cards[_currentCard++];
        }
    }
}
