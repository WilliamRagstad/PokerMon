using System;
using System.Collections.Generic;
using System.Net;

namespace PokerMon
{
    class Program
    {
        static Server Server;
        static void Main()
        {
            Functions.WriteColor("======= ", ConsoleColor.Blue); Functions.WriteColor("Poker Mon Client", ConsoleColor.Cyan); Functions.WriteLineColor(" =======", ConsoleColor.Blue);
            
            string name = Functions.ReadInput("Your Name").Replace(",",".");
            string serverip = Functions.ReadInput("Enter Server IP");

            if (IPAddress.TryParse(serverip, out IPAddress address))
            {
                Server = new Server();
                if (Server.Connect(name, address))
                {
                    // Connected, waiting for game to start
                    while(Server.OnlinePlayers.Count != Server.MaxPlayers)
                    {
                        Functions.LoadingStep($"Waiting for all players to join. ({Server.OnlinePlayers.Count}/{Server.MaxPlayers})");
                        System.Threading.Thread.Sleep(700);
                    }
                }
            }
            else
            {
                Functions.WriteLineColor("Invalid IP address!", ConsoleColor.Red);
            }
            Console.ReadKey(true);
        }

        static void ConnectToServer()
        {

        }
    }
}
