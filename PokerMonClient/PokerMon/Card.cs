using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMon
{
    enum CRank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
    enum CSuit
    {
        Heart,
        Diamonds,
        Clubs,
        Spades
    }

    struct Card
    {
        public CRank Rank;
        public CSuit Suit;

        public Card(CRank rank, CSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
