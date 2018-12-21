using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace H1Chess.Pieces
{
    [Serializable()]
    class Pawn : Piece
    {
        bool hasMoved = false;

        public Pawn(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\pawn_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\pawn_white.png");
        }

        public override string GetRepresentation()
        {
            return "pawn";
        }

        public override bool IsValidMove(ChessBoard board, Vector startPosition, Vector endPosition)
        {
            Vector directionVector = endPosition - startPosition;

            // No matter the endPosition, we cannot move onto a piece of our own color
            if (board.GetPieceAt(endPosition) != null && board.GetPieceAt(endPosition).GetColor() == GetColor())
                return false;

            // If we move down on the board, but the vector is up, it's not a valid move and vice versa
            if (GetDirection() == MoveDirection.Down && directionVector.Y <= 0)
                return false;
            if (GetDirection() == MoveDirection.Up && directionVector.Y >= 0)
                    return false;

            // We cannot ever move more than one unit to the side
            if (Math.Abs(directionVector.X) > 1)
                return false;

            // We cannot move more than one vertical unit if we already have moved
            if (Math.Abs(directionVector.Y) > 1 && hasMoved)
                return false;

            // If we move two squares, we cannot move to the side
            if (Math.Abs(directionVector.Y) == 2 && directionVector.X != 0)
                return false;

            // If we move to the side, we cannot move more than one unit, but only if there is an opponent there
            if (Math.Abs(directionVector.X) == 1 && board.GetPieceAt(endPosition) == null)
                return false;

            // Check we are not the same color when moving directional
            if (Math.Abs(directionVector.X) == 1 && board.GetPieceAt(endPosition).GetColor() == GetColor())
                return false;

            hasMoved = true;
            return true;
        }
    }
}
