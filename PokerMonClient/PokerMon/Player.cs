using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMon
{
    class Player
    {
        public string Name;
        public int Chips;
        public RoundStatus Status;
        public Card[] Hand;

        public Player(string name, int handSize, int chips)
        {
            Name = name;
            Chips = chips;
            Status = RoundStatus.Unset;
            Hand = new Card[handSize];
        }

    }
}
