using System;
using System.Drawing;
using System.Windows;

namespace H1Chess.Pieces
{
    [Serializable()]
    class Bishop : Piece
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

        public override bool IsValidMove(ChessBoard board, Vector startPosition, Vector endPosition)
        {
            // Moving the brick to the same position is no movement and therefore not a valid move
            if (startPosition == endPosition)
                return false;

            // The direction that the piece will move in
            Vector directional = endPosition - startPosition;
            directional.Normalize();

            // If it is not a diagonal angle, then it is not valid
            if (Vector.AngleBetween(new Vector(1, 0), directional) % 90 != 45)
                return false;

            // We transform the direction into full steps (and we can do this, cuz we know by now it's actually a diagonal)
            directional = new Vector(Math.Ceiling(directional.X), Math.Ceiling(directional.Y));

            // Move the brick and see if there are pieces inbetween
            Vector step = startPosition + directional;
            while (step != endPosition)
            {
                if (board.GetPieceAt(step) != null)
                {
                    return false;
                }
                step += directional;
            }

            // If it is a piece of same color as this brick, don't move atop it
            if (board.GetPieceAt(endPosition) != null && board.GetPieceAt(endPosition).GetColor() == GetColor())
                return false;

            return true;
        }
    }
}