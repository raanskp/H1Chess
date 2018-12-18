using System;
using System.Drawing;

namespace H1Chess.Pieces
{
    internal class Bishop : Piece
    {
        public Bishop(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\bishop_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\bishop_white.png");
        }

        public override string GetRepresentation()
        {
            return "bishop";
        }

        public override bool IsValidMove(ChessBoard board, Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            return false;
        }
    }
}