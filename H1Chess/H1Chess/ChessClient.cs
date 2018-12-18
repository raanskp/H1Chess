using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace H1Chess
{
    class ChessClient : TcpClient
    {
        byte[] buffer = new byte[512];

        public ChessClient()
        {

        }        

        public ChessBoard GetBoard()
        {
            byte[] getBoardMessage = new byte[] { 1, 0 };

            GetStream().Write(getBoardMessage, 0, getBoardMessage.Length);
            GetStream().Read(buffer, 0, buffer.Length);

            for (int i = 0; i < buffer.Length; i++)
            {
                Console.Write("0x" + buffer[i].ToString("X") + ", ");

                if (buffer[i] == 0)
                {
                    break;
                }
            }

            ChessBoard board = new ChessBoard();
            return board;
        }
    }
}
