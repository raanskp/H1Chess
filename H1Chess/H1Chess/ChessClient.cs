using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace H1Chess
{
    class ChessClient : TcpClient
    {
        private ChessBoard board;
        byte[] buffer = new byte[512];

        public ChessClient() { }        
         
        public async Task RunAsync()
        {
            bool isRunning = true;

            while(isRunning)
            {
                string message = Encoding.UTF8.GetString(await GetMessageAsync());
                string command = message.Split(' ')[0];
                string parameters = "";

                if (message.Split(' ').Length > 1)
                    parameters = message.Split(' ')[1];

                switch (command)
                {
                    case "movepieceok":
                        // Eventually we'd want to handle individual piece movement instead of just requesting the full board
                        // over and over again.
                        break;
                    case "board":
                        board = new ChessBoard(parameters);
                        break;
                    default:
                        // Unknown command "handler"
                        break;
                }
            }
        }

        /// <summary>
        /// Helper function to send a request to the server to move a piece. As with all other things it is async and
        /// the main listener loop will listen for a response if the move was OK.
        /// </summary>
        /// <param name="from">Start coordinate</param>
        /// <param name="to">End coordinate</param>
        public void MovePiece(Vector from, Vector to)
        {
            SendMessageAsync(string.Format("movepiece {0},{1} {2},{3}", from.X, from.Y, to.X, to.Y));
        }

        /// <summary>
        /// Returns the current board as it is. There can be up to a 5 seconds delay on the first connecting while waiting for the
        /// board to load.
        /// </summary>
        /// <returns>The server chess board or null if there was no response within 5 seconds.</returns>
        public ChessBoard GetBoard()
        {
            int attempts = 0;

            SendMessageAsync("getboard");

            while(board == null && attempts < 20)
            {
                System.Threading.Thread.Sleep(250);
                attempts++;
            }

            return board;
        }

        /// <summary>
        /// Helper function to send a message to the server we are connected to.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns></returns>
        private async Task SendMessageAsync(string message)
        {
            // Allocate a buffer one larger than the encoded message (for a null terminator)
            byte[] buffer = new byte[Encoding.UTF8.GetByteCount(message) + 1];

            Encoding.UTF8.GetBytes(message, 0, message.Length, buffer, 0);
            await GetStream().WriteAsync(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Helper function to get a message from the network stream. Messages are seperated by the byte value 0. 
        /// </summary>
        /// <returns>A byte array holding the message recieved.</returns>
        private async Task<byte[]> GetMessageAsync()
        {
            List<byte> message = new List<byte>();
            bool gotFullMessage = false;

            // TODO: Add message size limit
            while (!gotFullMessage)
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
                if (!gotFullMessage || message.Count == 0)
                {
                    gotFullMessage = false;
                    buffer = new byte[buffer.Length];
                    int length = await GetStream().ReadAsync(buffer, 0, buffer.Length);
                }
            }

            // Move the rest of the elements in the buffer back to index 0 and forward
            int startPosition = message.Count % buffer.Length;
            byte start = buffer[0];
            for (int i = startPosition + 1; i < buffer.Length; i++)
            {
                buffer[i - (startPosition + 1)] = buffer[i];
                buffer[i] = 0;
            }

            return message.ToArray();
        }
    }
}
