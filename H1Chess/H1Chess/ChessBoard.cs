using System;

namespace H1Chess
{
    class ChessBoard
    {
        private const int BOARD_SIZE = 8;

        private Piece[,] board;
        
        public ChessBoard()
        {
            board = new Piece[BOARD_SIZE, BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                board[1, i] = new Pieces.Pawn(MoveDirection.Up, PieceColor.White);
                board[6, i] = new Pieces.Pawn(MoveDirection.Down, PieceColor.Black);
            }

            board[0, 4] = new Pieces.King(MoveDirection.Up, PieceColor.White);
            board[7, 4] = new Pieces.King(MoveDirection.Down, PieceColor.Black);

            board[0, 3] = new Pieces.Queen(MoveDirection.Up, PieceColor.White);
            board[7, 3] = new Pieces.Queen(MoveDirection.Down, PieceColor.Black);

            board[0, 2] = new Pieces.Bishop(MoveDirection.Up, PieceColor.White);
            board[0, 5] = new Pieces.Bishop(MoveDirection.Up, PieceColor.White);
            board[7, 5] = new Pieces.Bishop(MoveDirection.Down, PieceColor.Black);
            board[7, 2] = new Pieces.Bishop(MoveDirection.Down, PieceColor.Black);

            board[0, 1] = new Pieces.Knight(MoveDirection.Up, PieceColor.White);
            board[0, 6] = new Pieces.Knight(MoveDirection.Up, PieceColor.White);
            board[7, 6] = new Pieces.Knight(MoveDirection.Down, PieceColor.Black);
            board[7, 1] = new Pieces.Knight(MoveDirection.Down, PieceColor.Black);

            board[0, 0] = new Pieces.Rook(MoveDirection.Up, PieceColor.White);
            board[0, 7] = new Pieces.Rook(MoveDirection.Up, PieceColor.White);
            board[7, 7] = new Pieces.Rook(MoveDirection.Down, PieceColor.Black);
            board[7, 0] = new Pieces.Rook(MoveDirection.Down, PieceColor.Black);
        }

        public Piece GetPieceAt(int x, int y)
        {
            return board[y, x];
        }
    }
}