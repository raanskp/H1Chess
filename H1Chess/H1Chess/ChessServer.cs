using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace H1Chess
{
    class ChessServer : TcpListener
    {
        ChessBoard board;
        bool isRunning = true;

        ConcurrentQueue<TcpClient> playerQueue = new ConcurrentQueue<TcpClient>();

        public ChessServer(IPEndPoint localEP) : base(localEP)
        {
            board = new ChessBoard();

            

            while(true)
            {

            }
        }

        public async Task AcceptChessClientsAsync()
        {
            while (isRunning)
            {
                TcpClient client = await AcceptTcpClientAsync();


            }
        }

    }
}
