using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Chess.Pieces
{
    [Serializable()]
    class Pawn : Piece
    {
        public Pawn(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\pawn_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\pawn_white.png");
        }

        public override string GetRepresentation()
        {
            return "pawn";
        }

        public override bool IsValidMove(ChessBoard board, Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            if (GetDirection() == MoveDirection.Down)
            {
                if (endPosition.Item2 >= startPosition.Item2)
                    return false;

                if (board.GetPieceAt(startPosition.Item1, startPosition.Item2 - 1 ) != null )
                    return false;
            }

            if (GetDirection() == MoveDirection.Up)
            {
                if (endPosition.Item2 <= startPosition.Item2)
                    return false;

                if (board.GetPieceAt(startPosition.Item1, startPosition.Item2 + 1) != null)
                    return false;
            }

            return true;
        }
    }
}
