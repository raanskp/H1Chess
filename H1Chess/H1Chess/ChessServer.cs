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

        ConcurrentQueue<TcpClient> playerQueue = new ConcurrentQueue<TcpClient>();

        public ChessServer(IPEndPoint localEP) : base(localEP)
        {
            board = new ChessBoard();
            board.MovePiece(1, 1, 1, 2);
        }
        
        public async Task Run()
        {
            await AcceptChessClientsAsync();
            isRunning = true;

            while (isRunning)
            {
                foreach (TcpClient client in playerQueue)
                {
                    
                }
                System.Threading.Thread.Sleep(100);
            }
        }

        public new void Stop()
        {
            base.Stop();
            Console.WriteLine("Stopping server");
            isRunning = false;
        }

        public async Task ClientListener(TcpClient client)
        {
            byte[] buffer = new byte[256];

            while (true)
            {
                Console.WriteLine("Waiting for message...");
                Data message = await GetMessage(buffer, client);
                buffer = message.buffer;
                Console.WriteLine("Got message: " + Encoding.UTF8.GetString(message.message));

                switch (Encoding.UTF8.GetString(message.message).Split(' ')[0])
                {
                    case "getboard":
                        BinaryFormatter f = new BinaryFormatter();
                        f.Serialize(client.GetStream(), board);
                        Console.WriteLine("Done serializing");
                        break;
                    case "movepiece":
                        Console.WriteLine("Got move request");
                        int startX = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[1].Split(',')[0]);
                        int startY = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[1].Split(',')[1]);
                        int endX = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[2].Split(',')[0]);
                        int endY = int.Parse(Encoding.UTF8.GetString(message.message).Split(' ')[2].Split(',')[1]);
                        bool result = board.MovePiece(startX, startY, endX, endY);
                        if (result)
                        {
                            SendMessage(client.GetStream(), "OK");
                        }
                        else
                        {
                            SendMessage(client.GetStream(), "BAD");
                        }
                        break;
                    default:
                        client.GetStream().Write(Encoding.UTF8.GetBytes("Unknown message"), 0, Encoding.UTF8.GetBytes("Unknown message").Length);
                        break;
                }
            }
        }

        private void SendMessage(NetworkStream stream, string message)
        {
            // Allocate a buffer one larger than the encoded message (for a null terminator)
            byte[] buffer = new byte[Encoding.UTF8.GetByteCount(message) + 1];

            Encoding.UTF8.GetBytes(message, 0, message.Length, buffer, 0);
            stream.Write(buffer, 0, buffer.Length);
        }

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

            int startPosition = message.Count % buffer.Length;
            byte start = buffer[0];
            for (int i = startPosition + 1; i < buffer.Length; i++)
            {
                buffer[i - (startPosition + 1)] = buffer[i];
                buffer[i] = 0;
            }

            return new Data(message.ToArray(), buffer);
        }

        private void WriteBoard(NetworkStream networkStream)
        {
            byte[] boardBuffer = new byte[256];
            int bufferIndex = 0;

            for (byte y = 0; y < 8; y++)
            {
                for (byte x = 0; x < 8; x++)
                {
                    boardBuffer[bufferIndex++] = y;
                    boardBuffer[bufferIndex++] = x;
                }
            }
        }

        public async Task AcceptChessClientsAsync()
        {
            while (isRunning)
            {
                Console.WriteLine("Awaiting clients");
                TcpClient client = await AcceptTcpClientAsync();
                Console.WriteLine("Client has connected to the chess server (" + client.Client.RemoteEndPoint + ")");
                await ClientListener(client);
                playerQueue.Enqueue(client);
            }
        }

    }
}
