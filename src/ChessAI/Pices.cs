using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    public abstract class APiece
    {
        public abstract PiceType Type { get; }
        public bool IsWhite { get; set; }
        public virtual IEnumerable<Board> PossibleMoves(Board currentBoard, byte row, byte col)
        {
            var currentLoc = new Loc(row, col);
            foreach (var loc in TilesCovered(currentBoard, row, col))
            {
                if (!currentBoard.IsOccupied(loc, IsWhite))
                    yield return currentBoard.WithNewLocation(currentLoc, loc);
            }
        }
        public abstract IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col);
    }
    public class King : APiece
    {
        public override PiceType Type => PiceType.King;
        public override IEnumerable<Board> PossibleMoves(Board currentBoard, byte row, byte col)
        {
            var currentLoc = new Loc(row, col);
            var boards = new List<Board>();
            foreach (var loc in TilesCovered(currentBoard, row, col))
            {
                if (!currentBoard.IsCovered(IsWhite, loc))
                {
                    boards.Add(currentBoard.WithNewLocation(currentLoc, loc));
                }
            }
            return boards;
        }
        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col)
        {
            for (sbyte i = -1; i < 2; i++)
            {
                for (sbyte j = -1; j < 2; j++)
                {
                    if (row + i < 0 || row + i > 7) continue;
                    if (col + j < 0 || col + j > 7) continue;
                    var loc = new Loc((byte)(row + i), (byte)(col + j));
                    if (!currentBoard.IsOccupied(loc, IsWhite))
                    {
                        yield return loc;
                    }
                }
            }
        }
    }

    public class Pawn : APiece
    {
        public override PiceType Type => PiceType.Pawn;

        public override IEnumerable<Board> PossibleMoves(Board currentBoard, byte row, byte col)
        {
            var direction = -1;
            if (IsWhite) direction = 1;

            if (row + direction < 0 || row + direction > 7)
                yield break;

            var newLoc = new Loc(row + direction, col);
            var currentLoc = new Loc(row, col);
            if (!currentBoard.IsOccupied(newLoc))
            {
                yield return currentBoard.WithNewLocation(currentLoc, newLoc);
            }
        }

        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col)
        {
            var direction = -1;
            if (IsWhite) direction = 1;

            if (row + direction < 0 || row + direction > 7)
                yield break;
            if (col < 7)
                yield return new Loc(row + direction, col + 1);
            if (col > 0)
                yield return new Loc(row + direction, col - 1);
        }
    }
    public class Rook : APiece
    {
        public override PiceType Type => PiceType.Rook;

        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col) => _TilesCovered(currentBoard, row, col);
        public static IEnumerable<Loc> _TilesCovered(Board currentBoard, byte row, byte col)
        {
            var covered = new List<Loc>();
            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row + i, col);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }

            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row - i, col);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }

            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row, col + i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }

            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row, col - i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }
            return covered;
        }
    }
    public class Knight : APiece
    {
        public override PiceType Type => PiceType.Knight;

        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col)
        {
            var locs = new[]
            {
                new Loc(row + 2, col + 1),
                new Loc(row + 2, col - 1),
                new Loc(row - 2, col + 1),
                new Loc(row - 2, col - 1),
                new Loc(row + 1, col + 2),
                new Loc(row + 1, col - 2),
                new Loc(row - 1, col + 2),
                new Loc(row - 1, col - 2),
            };

            foreach (var loc in locs)
            {
                if (Board.InsideBoard(loc))
                {
                    yield return loc;
                }
            }
        }
    }
    public class Bishop : APiece
    {
        public override PiceType Type => PiceType.Bishop;

        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col) => _TilesCovered(currentBoard, row, col);
        public static IEnumerable<Loc> _TilesCovered(Board currentBoard, byte row, byte col)
        {
            var covered = new List<Loc>();
            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row + i, col + i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }
            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row + i, col - i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }
            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row - i, col + i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }
            for (int i = 1; i < 8; i++)
            {
                var loc = new Loc(row - i, col - i);
                if (!Board.InsideBoard(loc)) break;
                covered.Add(loc);
                if (currentBoard.IsOccupied(loc)) break;
            }
            return covered;
        }
    }
    public class Queen : APiece
    {
        public override PiceType Type => PiceType.Queen;

        public override IEnumerable<Loc> TilesCovered(Board currentBoard, byte row, byte col)
            => Rook._TilesCovered(currentBoard, row, col).Union(Bishop._TilesCovered(currentBoard, row, col));
    }
}
