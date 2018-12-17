using System;

namespace H1Chess
{
    abstract class Piece
    {
        MoveDirection direction;
        PieceColor color;

        public Piece(MoveDirection direction, PieceColor color)
        {
            this.direction = direction;
            this.color = color;
        }
        public MoveDirection GetDirection()
        {
            return direction;
        }
        public PieceColor GetColor()
        {
            return color;
        }
        public abstract string GetRepresentation();
        public abstract bool IsValidMove(ChessBoard board, Tuple<int, int> startPosition, Tuple<int, int> endPosition);
    }

    enum MoveDirection
    {
        Up,
        Down
    }

    enum PieceColor
    {
        Black,
        White
    }
}