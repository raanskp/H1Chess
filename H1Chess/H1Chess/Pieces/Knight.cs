using System;
using System.Drawing;

namespace H1Chess.Pieces
{
    internal class Knight : Piece
    {
        public Knight(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\knight_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\knight_white.png");
        }

        public override string GetRepresentation()
        {
            return "knight";
        }

        public override bool IsValidMove(ChessBoard board, Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            return false;
        }
    }
}