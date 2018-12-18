using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace H1Chess
{
    class ChessServer : TcpListener
    {
        ChessBoard board = new ChessBoard();
        bool isRunning = true;

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
            int bytesRead = 1;
            byte[] buffer = new byte[256];
            IFormatter formatter = new BinaryFormatter();

            while (bytesRead > 0)
            {
                bytesRead = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Got message");
                //writeBoard(client.GetStream());
            }
        }

        private void writeBoard(NetworkStream networkStream)
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
