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
        public ChessClient()
        {

        }        

        public ChessBoard GetBoard()
        {
            ChessBoard board = new ChessBoard();
            return board;
        }
    }
}
