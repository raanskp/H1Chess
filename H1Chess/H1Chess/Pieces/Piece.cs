using System;
using System.Drawing;

namespace H1Chess
{
    [Serializable()]
    abstract class Piece
    {
        protected MoveDirection direction;
        protected PieceColor color;
        protected Image pieceImageBlack;
        protected Image pieceImageWhite;

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

        public Image GetImage()
        {
            if (GetColor() == PieceColor.Black)
                return pieceImageBlack;

            return pieceImageWhite;
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