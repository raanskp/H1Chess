using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace H1Chess
{
    // We cannot pass references to async methods, so we make a Data class that holds the message 
    // recieved and the buffer that's left over
    class Data
    {
        public byte[] message;
        public byte[] buffer;

        public Data(byte[] message, byte[] buffer)
        {
            this.message = message;
            this.buffer = buffer;
        }
    }

    class ChessServer : TcpListener
    {
        ChessBoard board = new ChessBoard();
        bool isRunning = true;

        // We use a thread safe queue for the players that connect, because we'd eventually want to make it possible for 
        // multiple pairs of players to connect. They will then be placed in a queue waiting for another player to connect.
        ConcurrentQueue<TcpClient> playerQueue = new ConcurrentQueue<TcpClient>();

        public ChessServer(IPEndPoint localEP) : base(localEP)
        {
            board = new ChessBoard();
        }
        
        public async Task Run()
        {
            await AcceptChessClientsAsync();
            isRunning = true;

            while (isRunning)
            {
                foreach (TcpClient client in playerQueue)
                {
                    // Anything we'd want to do for each connected player, but which is not handled in the main listener loop.
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public new void Stop()
        {
            base.Stop();
            isRunning = false;
        }


        /// <summary>
        /// Main listener thread for each client.
        /// </summary>
        /// <param name="client">The client we are handling</param>
        /// <returns></returns>
        public async Task ClientListener(TcpClient client)
        {
            byte[] buffer = new byte[256];

            while (isRunning)
            {
                Data message = await GetMessage(buffer, client);
                buffer = message.buffer;

                switch (Encoding.UTF8.GetString(message.message).Split(' ')[0])
                {
                    case "getboard":
                        SendMessageAsync(client.GetStream(), "board " + board.GetNetworkString());
                        break;
                    case "movepiece":
                        int startX = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[1].Split(',')[0]);
                        int startY = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[1].Split(',')[1]);
                        int endX = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[2].Split(',')[0]);
                        int endY = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[2].Split(',')[1]);
                        bool result = board.MovePiece(startX, startY, endX, endY);
                        if (result)
                        {
                            SendMessageAsync(client.GetStream(), string.Format("moveok {0},{1},{2},{3}", startX, startY, endX, endY));
                        }
                        else
                        {
                            SendMessageAsync(client.GetStream(), string.Format("movebad {0},{1},{2},{3}", startX, startY, endX, endY));
                        }
                        break;
                    default:
                        SendMessageAsync(client.GetStream(), "badmessage");
                        break;
                }
            }
        }

        private async Task SendMessageAsync(NetworkStream stream, string message)
        {
            // Allocate a buffer one larger than the encoded message (for a null terminator)
            byte[] buffer = new byte[Encoding.UTF8.GetByteCount(message) + 1];

            Encoding.UTF8.GetBytes(message, 0, message.Length, buffer, 0);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Helper function to get a message from the stream. Messages are seperated by the byte value 0.
        /// </summary>
        /// <param name="buffer">The buffer that holds the rest of the old message.</param>
        /// <param name="client">The client we are listening to.</param>
        /// <returns></returns>
        private async Task<Data> GetMessage(byte[] buffer, TcpClient client)
        {
            List<byte> message = new List<byte>();
            bool gotFullMessage = false;

            // TODO: Add message size limit
            while(!gotFullMessage)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    // Add to the message so long as it is not 0
                    if (buffer[i] == 0)
                    {
                        gotFullMessage = true;
                        break;
                    }
                    message.Add(buffer[i]);
                }

                // If we reached the end of the buffer with no null, then we need to read again
                if (!gotFullMessage || message.Count == 0){
                    gotFullMessage = false;
                    buffer = new byte[buffer.Length];
                    int length = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                }
            }
            
            // Move all the elements in the array back towards index 0
            int startPosition = message.Count % buffer.Length;
            byte start = buffer[0];
            for (int i = startPosition + 1; i < buffer.Length; i++)
            {
                buffer[i - (startPosition + 1)] = buffer[i];
                buffer[i] = 0;
            }

            return new Data(message.ToArray(), buffer);
        }

        /// <summary>
        /// Handler function for each client that connects.
        /// </summary>
        /// <returns></returns>
        public async Task AcceptChessClientsAsync()
        {
            while (isRunning)
            {
                Console.WriteLine("Awaiting clients");
                TcpClient client = await AcceptTcpClientAsync();
                Console.WriteLine("Client has connected to the chess server (" + client.Client.RemoteEndPoint + ")");
                ClientListener(client);
                playerQueue.Enqueue(client);
            }
        }

    }
}
