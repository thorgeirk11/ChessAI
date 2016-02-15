using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = Board.StartingBoard();

            Console.WriteLine(board);
            Console.WriteLine();

            foreach (var newBoard in board.PossibleMoves())
            {
                Console.WriteLine(newBoard);
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
