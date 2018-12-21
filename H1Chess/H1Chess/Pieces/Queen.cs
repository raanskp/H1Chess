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
    class Queen : Piece
    {
        public Queen(MoveDirection direction, PieceColor color) : base(direction, color)
        {
            pieceImageBlack = Bitmap.FromFile(@"Images\Pieces\queen_black.png");
            pieceImageWhite = Bitmap.FromFile(@"Images\Pieces\queen_white.png");
        }

        public override string GetRepresentation()
        {
            return "queen";
        }

        public override bool IsValidMove(ChessBoard board, Vector startPosition, Vector endPosition)
        {
            return false;
        }
    }
}
