using System;
using System.Drawing;

namespace H1Chess.Pieces
{
    internal class Rook : Piece
    {
        public Rook(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\rook_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\rook_white.png");
        }

        public override string GetRepresentation()
        {
            return "rook";
        }

        public override bool IsValidMove(ChessBoard board, Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            return false;
        }
    }
}