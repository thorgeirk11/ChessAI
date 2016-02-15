using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    public enum PiceType : byte
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public struct Loc
    {
        public Loc(int row, int col)
        {
            Row = (byte)row;
            Col = (byte)col;
        }

        public byte Row { get; }
        public byte Col { get; }
        public override string ToString() => $"{(char)(Row + 97)}{Col}";
    }

    public class Board
    {
        private Board(Dictionary<Loc, APiece> piecs)
        {
            Pieces = piecs;
        }

        public bool IsOccupied(Loc loc) => IsOccupied(loc, true) || IsOccupied(loc, false);
        public bool IsOccupied(Loc loc, bool white) =>
            Pieces.ContainsKey(loc) ? Pieces[loc].IsWhite == white : false;

        public Dictionary<Loc, APiece> Pieces { get; }

        public Board WithNewLocation(Loc loc, Loc newLoc)
        {
            var newPices = Pieces.ToDictionary(i => i.Key, i => i.Value);
            newPices[newLoc] = newPices[loc];
            newPices.Remove(loc);
            return new Board(newPices);
        }

        public static bool InsideBoard(Loc location) =>
            location.Row >= 0 && location.Row <= 7 &&
            location.Col >= 0 && location.Col <= 7;

        public bool IsCovered(bool white, Loc loc) => CoveredTiles(white).Contains(loc);

        public IEnumerable<Board> PossibleMoves() =>
            from p in Pieces
            from board in p.Value.PossibleMoves(this, p.Key.Row, p.Key.Col)
            select board;

        public IEnumerable<Loc> CoveredTiles(bool white) =>
            from p in Pieces
            where p.Value.IsWhite == white
            from loc in p.Value.TilesCovered(this, p.Key.Row, p.Key.Col)
            select loc;

        public bool IsCheck
        {
            get
            {
                var kings = Pieces.Where(i => i.Value.Type == PiceType.King);
                foreach (var king in kings)
                {
                    var loc = king.Key;
                    var white = king.Value.IsWhite;
                    if (CoveredTiles(!white).Contains(loc)) return true;
                }
                return false;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (byte row = 0; row < 8; row++)
            {
                sb.Append(row + " ");
                for (byte col = 0; col < 8; col++)
                {
                    var loc = new Loc(row, col);
                    if (Pieces.ContainsKey(loc))
                        sb.Append(GetName(Pieces[loc].Type) + " ");
                    else
                        sb.Append("  ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        private static string GetName(PiceType type)
        {
            switch (type)
            {
                case PiceType.Pawn: return "P";
                case PiceType.Rook: return "R";
                case PiceType.Knight: return "N";
                case PiceType.Bishop: return "B";
                case PiceType.Queen: return "Q";
                case PiceType.King: return "K";
                default: return " ";
            }
        }

        #region Starting Board
        public static Board StartingBoard() => new Board(new Dictionary<Loc, APiece>
        {
            {  new Loc(0,0), new Rook { IsWhite = true } },
            {  new Loc(0,1), new Knight { IsWhite = true } },
            {  new Loc(0,2), new Bishop { IsWhite = true } },
            {  new Loc(0,3), new King { IsWhite = true } },
            {  new Loc(0,4), new Queen { IsWhite = true } },
            {  new Loc(0,5), new Bishop { IsWhite = true } },
            {  new Loc(0,6), new Knight { IsWhite = true } },
            {  new Loc(0,7), new Rook { IsWhite = true } },

            {  new Loc(1,0), new Pawn { IsWhite = true } },
            {  new Loc(1,1), new Pawn { IsWhite = true } },
            {  new Loc(1,2), new Pawn { IsWhite = true } },
            {  new Loc(1,3), new Pawn { IsWhite = true } },
            {  new Loc(1,4), new Pawn { IsWhite = true } },
            {  new Loc(1,5), new Pawn { IsWhite = true } },
            {  new Loc(1,6), new Pawn { IsWhite = true } },
            {  new Loc(1,7), new Pawn { IsWhite = true } },


            {  new Loc(6,0), new Pawn { IsWhite = false } },
            {  new Loc(6,1), new Pawn { IsWhite = false } },
            {  new Loc(6,2), new Pawn { IsWhite = false } },
            {  new Loc(6,3), new Pawn { IsWhite = false } },
            {  new Loc(6,4), new Pawn { IsWhite = false } },
            {  new Loc(6,5), new Pawn { IsWhite = false } },
            {  new Loc(6,6), new Pawn { IsWhite = false } },
            {  new Loc(6,7), new Pawn { IsWhite = false } },

            {  new Loc(7,0), new Rook { IsWhite = false } },
            {  new Loc(7,1), new Knight { IsWhite = false } },
            {  new Loc(7,2), new Bishop { IsWhite = false } },
            {  new Loc(7,3), new King { IsWhite = false } },
            {  new Loc(7,4), new Queen { IsWhite = false } },
            {  new Loc(7,5), new Bishop { IsWhite = false } },
            {  new Loc(7,6), new Knight { IsWhite = false } },
            {  new Loc(7,7), new Rook { IsWhite = false } },
        });
        #endregion
    }
}
