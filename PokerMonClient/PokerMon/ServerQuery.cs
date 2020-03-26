using System;
using System.Collections.Generic;
using System.Text;

namespace PokerMon
{
    static class ServerQuery
    {
        public static string Join(string name) => $"join|{name}";
    }
}
