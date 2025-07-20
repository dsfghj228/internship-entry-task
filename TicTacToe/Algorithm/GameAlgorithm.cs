using TicTacToe.Enums;
using TicTacToe.Models;

namespace TicTacToe.Algorithm
{
    public class GameAlgorithm
    {
        public static bool VerticalCheck(List<List<Cell>> board, int size)
        {
            for (int col = 0; col < size; col++)
            {
                var firstSymbol = board[0][col];
                if (firstSymbol == Cell.Empty)
                {
                    continue;
                }

                var win = true;

                for (int row = 1; row < size; row++)
                {
                    if (board[row][col] != firstSymbol)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public static bool HorizontalCheck(List<List<Cell>> board, int size)
        {
            for (int row = 0; row < size; row++)
            {
                var firstSymbol = board[row][0];
                if (firstSymbol == Cell.Empty)
                {
                    continue;
                }
                var win = true;
                for (int col = 1; col < size; col++)
                {
                    if (board[row][col] != firstSymbol)
                    {
                        win = false;
                        break;
                    }
                }

                if (win)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public static bool CrossCheck(List<List<Cell>> board, int size)
        {
            var firstSymbol = board[0][0];
            if (firstSymbol != Cell.Empty)
            {
                bool mainDiagonalWin = true;
                for (int i = 1; i < size; i++)
                {
                    if (board[i][i] != firstSymbol)
                    {
                        mainDiagonalWin = false;
                        break;
                    }
                }
                if (mainDiagonalWin)
                    return true;
            }
            
            firstSymbol = board[0][size - 1];
            if (firstSymbol != Cell.Empty)
            {
                bool antiDiagonalWin = true;
                for (int i = 1; i < size; i++)
                {
                    if (board[i][size - i - 1] != firstSymbol)
                    {
                        antiDiagonalWin = false;
                        break;
                    }
                }
                
                if (antiDiagonalWin)
                    return true;
            }
            
            return false;
        }
    }
}