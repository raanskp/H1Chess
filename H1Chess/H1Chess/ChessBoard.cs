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
        }

        public Piece GetPieceAt(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}