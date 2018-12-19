using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
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

        public void MovePiece(Tuple<int, int> from, Tuple<int, int> to)
        {
            Console.WriteLine(string.Format("movepiece {0},{1} {2},{3}", from.Item1, from.Item2, to.Item1, to.Item2));
            SendMessage(string.Format("movepiece {0},{1} {2},{3}", from.Item1, from.Item2, to.Item1, to.Item2));
            byte[] buffer = new byte[256];
            //GetStream().Read(buffer, 0, buffer.Length);
            Console.WriteLine("Response was: " + Encoding.UTF8.GetString(buffer));
        }

        public ChessBoard GetBoard()
        {
            // Tell the server we want to get the full board
            SendMessage("getboard");

            BinaryFormatter b = new BinaryFormatter();
            
            ChessBoard board = (ChessBoard)b.Deserialize(GetStream());
            return board;
        }

        private void SendMessage(string message)
        {
            // Allocate a buffer one larger than the encoded message (for a null terminator)
            byte[] buffer = new byte[Encoding.UTF8.GetByteCount(message) + 1];

            Encoding.UTF8.GetBytes(message, 0, message.Length, buffer, 0);
            GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
