using System;
using System.Windows;

namespace H1Chess
{
    [Serializable()]
    class ChessBoard
    {
        private const int BOARD_SIZE = 8;
        public Piece[,] board;
        
        /// <summary>
        /// This constructor will initialize the ChessBoard as a standard chessboard.
        /// </summary>
        public ChessBoard()
        {
            board = new Piece[BOARD_SIZE, BOARD_SIZE];

            // A lot of pawns
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                board[1, i] = new Pieces.Pawn(MoveDirection.Up, PieceColor.White);
                board[6, i] = new Pieces.Pawn(MoveDirection.Down, PieceColor.Black);
            }

            // Two kings
            board[0, 4] = new Pieces.King(MoveDirection.Up, PieceColor.White);
            board[7, 4] = new Pieces.King(MoveDirection.Down, PieceColor.Black);

            // Two queens
            board[0, 3] = new Pieces.Queen(MoveDirection.Up, PieceColor.White);
            board[7, 3] = new Pieces.Queen(MoveDirection.Down, PieceColor.Black);

            // All the bishops
            board[0, 2] = new Pieces.Bishop(MoveDirection.Up, PieceColor.White);
            board[0, 5] = new Pieces.Bishop(MoveDirection.Up, PieceColor.White);
            board[7, 5] = new Pieces.Bishop(MoveDirection.Down, PieceColor.Black);
            board[7, 2] = new Pieces.Bishop(MoveDirection.Down, PieceColor.Black);

            // All the knights
            board[0, 1] = new Pieces.Knight(MoveDirection.Up, PieceColor.White);
            board[0, 6] = new Pieces.Knight(MoveDirection.Up, PieceColor.White);
            board[7, 6] = new Pieces.Knight(MoveDirection.Down, PieceColor.Black);
            board[7, 1] = new Pieces.Knight(MoveDirection.Down, PieceColor.Black);

            // All the rooks
            board[0, 0] = new Pieces.Rook(MoveDirection.Up, PieceColor.White);
            board[0, 7] = new Pieces.Rook(MoveDirection.Up, PieceColor.White);
            board[7, 7] = new Pieces.Rook(MoveDirection.Down, PieceColor.Black);
            board[7, 0] = new Pieces.Rook(MoveDirection.Down, PieceColor.Black);
        }

        public Piece GetPieceAt(Vector position)
        {
            return GetPieceAt((int)position.X, (int)position.Y);
        }

        /// <summary>
        /// This constructor will set up the chessboard in accordance to the encoded networkBoard string.
        /// </summary>
        /// <param name="networkBoard">An encoded string representing a ChessBoard</param>
        public ChessBoard(string networkBoard)
        {
            board = new Piece[BOARD_SIZE, BOARD_SIZE];

            // All the pieces are seperated with a _
            string[] pieces = networkBoard.Split('_');
            foreach (string piece in pieces)
            {
                if (piece == "")
                    continue; 

                string[] pieceInfo = piece.Split(',');

                // Handling numbers that are not numbers
                int x = 0;
                int y = 0;
                try
                {
                    x = int.Parse(pieceInfo[1]);
                    y = int.Parse(pieceInfo[2]);
                } catch (FormatException e)
                {
                    Console.WriteLine("Invalid position string for the piece " + piece);
                    continue;
                }
                
                // No positioning outside of the board
                if (x >= BOARD_SIZE || x < 0 || y >= BOARD_SIZE || y < 0)
                {
                    Console.WriteLine("Invalid position string for the piece " + piece);
                    continue;
                }

                PieceColor color = PieceColor.Black;
                if (pieceInfo[3] == "White")
                {
                    color = PieceColor.White;
                }

                MoveDirection direction = MoveDirection.Up;
                if (pieceInfo[4] == "Down")
                {
                    direction = MoveDirection.Down;
                }

                Piece newPiece = null;

                switch (pieceInfo[0])
                {
                    case "bishop":
                        newPiece = new Pieces.Bishop(direction, color);
                        break;
                    case "king":
                        newPiece = new Pieces.King(direction, color);
                        break;
                    case "knight":
                        newPiece = new Pieces.Knight(direction, color);
                        break;
                    case "pawn":
                        newPiece = new Pieces.Pawn(direction, color);
                        break;
                    case "queen":
                        newPiece = new Pieces.Queen(direction, color);
                        break;
                    case "rook":
                        newPiece = new Pieces.Rook(direction, color);
                        break;
                    default:
                        Console.WriteLine("Unknown piece " + piece);
                        continue;
                        break;
                }

                board[y, x] = newPiece;
            }
        }

        /// <summary>
        /// Returns the piece that is at the given location. 
        /// </summary>
        /// <param name="x">X-coordinate</param>
        /// <param name="y">Y-coordinate</param>
        /// <returns>A piece if there was one at the location, else null.</returns>
        public Piece GetPieceAt(int x, int y)
        {
            return board[y, x];
        }

        /// <summary>
        /// Moves a piece from a start position to an end position.
        /// </summary>
        /// <param name="startX">The start x-coordinate</param>
        /// <param name="startY">The start y-coordinate</param>
        /// <param name="endX">The end x-coordinate</param>
        /// <param name="endY">The end y-coordinate</param>
        /// <returns>A boolean value indicating if the move was successful or not.</returns>
        public bool MovePiece(int startX, int startY, int endX, int endY)
        {
            Piece currentPiece = GetPieceAt(startX, startY);

            if (currentPiece == null)
                return false;
                
            if (currentPiece.IsValidMove(this, new Vector(startX, startY), new Vector(endX, endY)))
            {
                SetPieceAt(endX, endY, currentPiece);
                SetPieceAt(startX, startY, null);
            }

            return true;
        }

        /// <summary>
        /// Helper function to set a piece in a specific location. Really just makes it all prettier than a direct array access.
        /// </summary>
        /// <param name="x">X-coordinate</param>
        /// <param name="y">Y-coordinate</param>
        /// <param name="currentPiece">The current piece that is set.</param>
        private void SetPieceAt(int x, int y, Piece currentPiece)
        {
            board[y, x] = currentPiece;
        }

        /// <summary>
        /// A function to get the ChessBoard as a string, that can be decoded, so we can send it over the network, because getting 
        /// BinaryFormatter to serialize a ChessBoard over a networkstream where messages are also being sent is tough and error prone.
        /// </summary>
        /// <returns>An encoded string representing the Chessboard</returns>
        public string GetNetworkString()
        {
            string networkBoard = "";
            int totalPieces = 0;

            for (int y = 0; y < BOARD_SIZE; y++)
            {
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    Piece p = board[y, x];
                    
                    if (p == null)
                        continue;

                    // We only need the underscore prefix if it is the first piece in the set
                    if (totalPieces == 0) 
                        networkBoard += string.Format("{0},{1},{2},{3},{4}", p.GetRepresentation(), x, y, p.GetColor(), p.GetDirection());
                    else 
                        networkBoard += string.Format("_{0},{1},{2},{3},{4}", p.GetRepresentation(), x, y, p.GetColor(), p.GetDirection());

                    totalPieces++;
                }
            }

            return networkBoard;
        }
    }
}