using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMonServer
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
        Hearts,
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

        public override string ToString() => $"{Rank} of {Suit}";
    }
}
