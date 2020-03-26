using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMonServer
{
    class DeckOutOfCardsException : Exception
    {
        public DeckOutOfCardsException() : base() { }
        public DeckOutOfCardsException(string message) : base(message) { }
    }
}
