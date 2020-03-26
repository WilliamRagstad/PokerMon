using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace PokerMonServer
{
    class Player
    {
        public Guid ID;
        public string Name;
        public int Chips;
        public RoundStatus Status;
        public Card[] Hand;

        public Socket Socket;
        public Player(Socket socket, string name, int chips, int handSize)
        {
            ID = Guid.NewGuid();
            Socket = socket;
            Name = name;
            Chips = chips;
            Status = RoundStatus.Unset;
            Hand = new Card[handSize];
        }
    }
}
