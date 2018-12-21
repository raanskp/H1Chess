using System;
using System.Drawing;
using System.Windows;

namespace H1Chess.Pieces
{
    [Serializable()]
    class Rook : Piece
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

        public override bool IsValidMove(ChessBoard board, Vector startPosition, Vector endPosition)
        {
            return false;
        }
    }
}