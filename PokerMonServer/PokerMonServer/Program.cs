using System;

namespace PokerMonServer
{
    partial class Program
    {
        static Server Server;
        static Deck Deck;
        static void Main()
        {
            Functions.WriteColor("======= ", ConsoleColor.Blue); Functions.WriteColor("Poker Mon Server", ConsoleColor.Cyan); Functions.WriteLineColor(" =======", ConsoleColor.Blue);
            int maxPlayers = Functions.ReadInput<int>("Players", 9);
            int startChips = Functions.ReadInput<int>("Start Chip Ammount", 100000);
            int handSize = Functions.ReadInput<int>("Hand size", 2);


            Server = new Server(maxPlayers, startChips, handSize);
            Server.Start();

            while(Server.Players.Count != maxPlayers)
            {
                Functions.LoadingStep("Waiting for players");
                System.Threading.Thread.Sleep(700);
            }

            // All players have joined!
            Functions.WriteLineColor("All players have joined!", ConsoleColor.Green);
            
            
            while(Server.IsRunning)
            {
                Console.Write("GAME LOOP");
                Console.CursorLeft = 0;
                System.Threading.Thread.Sleep(700);
            }
        }
    }
}
