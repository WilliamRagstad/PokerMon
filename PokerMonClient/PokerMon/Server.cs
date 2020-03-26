using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PokerMon
{
    class Server
    {
        public List<Player> OnlinePlayers;
        public Player LocalPlayer;
        public int MaxPlayers;

        private Guid _localPlayerID;
        private Socket _clientSocket;

        public Server()
        {
            _resetServer();
        }
        public bool Connect(string name, IPAddress address, int port = 6420, int maxAttempts = 10)
        {
            _resetServer();

            // Send alias and player ID to server and get an object of that player back, this is set to the LocalPlayer
            Functions.WriteColor("====== ", ConsoleColor.Blue); Functions.WriteColor("Joining match", ConsoleColor.Cyan); Functions.WriteLineColor(" ======", ConsoleColor.Blue);
            int attempts = 0;
            while(!_clientSocket.Connected)
            {
                attempts++;
                Functions.LoadingStep($"Connecting ({attempts}/{maxAttempts})");
                try
                {
                    _clientSocket.Connect(address, port);
                }
                catch (SocketException)
                {
                    Functions.WriteColor("(Failed) ", ConsoleColor.Red);
                }

                if (attempts >= maxAttempts)
                {
                    Functions.WriteLineColor("Could not join match hosted on: " + address);
                    return false;
                }
            }

            Functions.WriteLineColor("Connected to server!", ConsoleColor.Yellow);
            return _attemptJoin(name);
        }

        private void _resetServer()
        {
            MaxPlayers = -1;
            _localPlayerID = Guid.Empty;
            LocalPlayer = null;
            OnlinePlayers = new List<Player>();
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private bool _attemptJoin(string name)
        {
            // Get player  information and create a  local player
            string[] response = _send(ServerQuery.Join(name)).ToLower().Split('|');
            // Parse response
            if (response[0] == "joined")
            {
                Functions.WriteLineColor("You joined the match!", ConsoleColor.Green);

                int startHandSize = int.Parse(response[2]);
                int startChips = int.Parse(response[3]);

                // Create local player
                _localPlayerID = Guid.Parse(response[1]);
                LocalPlayer = new Player(name, startHandSize, startChips);

                string[] OtherPlayers = response[4].Split(',');
                for (int i = 0; i < OtherPlayers.Length; i++) OnlinePlayers.Add(new Player(OtherPlayers[i], startHandSize, startChips));
                

                return true;
            }
            else if (response[0] == "full") Functions.WriteLineColor("The match is full...", ConsoleColor.Red);
            else Functions.WriteLineColor("Unknown response: " + response[0], ConsoleColor.Red);

            _clientSocket.Disconnect(true);
            return false;
        }


        private string _send(string data)
        {
            _clientSocket.Send(Encoding.ASCII.GetBytes(data));
            byte[] response = new byte[1024];
            int size = _clientSocket.Receive(response);
            byte[] dataBuff = new byte[size]; // re-use data buffer
            Array.Copy(response, dataBuff, size);
            return Encoding.ASCII.GetString(dataBuff);
        }

        private void _sendAsync(string data, AsyncCallback handler)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            _clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, handler, null);
        }

        private void _receiveData(IAsyncResult AR)
        {
            _clientSocket.EndReceive(AR);
        }
    }
}
