using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PokerMonServer
{
    class Server
    {
        public List<Player> Players;
        public bool allowNewConnections = true;
        public bool IsRunning;


        private Socket _serverSocket;
        private List<Socket> _clientSockets;
        private byte[] _buffer;
        private readonly int playerCapacity;
        private readonly int startChips;
        private readonly int handSize;

        public Server(int playerCapacity, int startChips, int handSize)
        {
            Players = new List<Player>(playerCapacity);
            _buffer = new byte[1024];
            this.playerCapacity = playerCapacity;
            this.startChips = startChips;
            this.handSize = handSize;
        }

        public void Start(int port = 6420)
        {
            //  Setup Server
            Functions.WriteColor("====== ", ConsoleColor.Blue); Functions.WriteColor("Setting up server", ConsoleColor.Cyan); Functions.WriteLineColor(" ======", ConsoleColor.Blue);

            IsRunning = true;
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientSockets = new List<Socket>(); 
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(1);

            Functions.WriteColor("Hosting on (local): ");
            Functions.WriteColor(Functions.GetLocalIp().ToString(), ConsoleColor.Yellow);
            if (port != 6420) Functions.WriteColor(":" + port, ConsoleColor.Yellow);
            Console.WriteLine();
            Functions.WriteColor("Hosting on (global): ");
            Functions.WriteColor(Functions.GetExternalIp().ToString(), ConsoleColor.Yellow);
            if (port != 6420) Functions.WriteColor(":" + port, ConsoleColor.Yellow);
            Console.WriteLine();

            _serverSocket.BeginAccept(new AsyncCallback(_acceptCallback), null);
        }

        private void Stop()
        {
            IsRunning = false;
            Functions.WriteLineColor("Server is shutting down...", ConsoleColor.DarkYellow);
            _clientSockets.Clear();
            _serverSocket.Close();
        }

        private void _acceptCallback(IAsyncResult AR)
        {
            if (!IsRunning) return;
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);

            socket. (_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(_receiveCallback), socket);

            _serverSocket.BeginAccept(new AsyncCallback(_acceptCallback), null);
        }

        private void _receiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = 0;
            try
            {
                received = socket.EndReceive(AR);
            }
            catch(SocketException)  // Bug here?
            {
                // Dissconnect player
                for (int i = 0; i < Players.Count; i++)
                {
                    if (Players[i].Socket == socket)
                    {
                        _playerDisconnect(Players[i]);
                        return;
                    }
                }
            }

            byte[] dataBuff = new byte[received];
            Array.Copy(_buffer, dataBuff, received);

            string[] text = Encoding.ASCII.GetString(dataBuff).ToLower().Split('|');

            if (text[0] == "join")
            {
                if (allowNewConnections && Players.Count < playerCapacity)
                {
                    // Create a new player object
                    Player p = new Player(socket, text[1], startChips, handSize);

                    string playerList = string.Empty;
                    for (int i = 0; i < Players.Count; i++)
                    {
                        playerList += Players[i].Name;
                        if (i < Players.Count - 1) playerList += ',';
                    }
                    SendText(socket, ClientQuery.Joined(p.ID, handSize, startChips, playerList, playerCapacity));
                    
                    Players.Add(p);
                    Functions.WriteLineColor($"> {p.Name} connected!                 ", ConsoleColor.Green);
                }
                else if (Players.Count >= playerCapacity)
                {
                    allowNewConnections = false;
                    SendText(socket, ClientQuery.Full());
                }
                else
                {
                    SendText(socket, "not allowed");
                }
            }
            else
            {
                SendText(socket, "invalid");
            }
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(_receiveCallback), socket);
        }

        private void _sendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        private void _playerDisconnect(Player player)
        {
            Functions.WriteLineColor($"> {player.Name} disconnected.", ConsoleColor.Red);
            player.Socket.Disconnect(false);
            Players.Remove(player);

            if (Players.Count == 0) Stop();
        }

        private void SendText(Socket socket, string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(_sendCallback), socket);
        }
    }
}
