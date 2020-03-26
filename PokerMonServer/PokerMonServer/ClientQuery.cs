using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMonServer
{
    static class ClientQuery
    {
        public static string Joined(Guid id, int handSize, int startChips, string currentPlayers, int maxPlayers) => $"joined|{id}|{handSize}|{startChips}|{currentPlayers}|{maxPlayers}";

        public static string Full() => "full";
    }
}
