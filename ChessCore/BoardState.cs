using System.Collections.Generic;

namespace ChessCore
{
    public class BoardState
    {
        private object _scoreLock = new object();

        private Move _lastMove;
        private MinimaxResult _score;
        private Board _board;

        public BoardState(int width, int height, Move lastMove = null)
        {
            _board = new Board(width, height);
            _lastMove = lastMove;
        }

        public BoardState(Board board, Move lastMove = null)
        {
            _board = board;
            _lastMove = lastMove;
        }

        public Board GetBoard()
        {
            return _board;
        }

        public Move GetLastMove()
        {
            return _lastMove;
        }

        public MinimaxResult GetScore()
        {
            lock (_scoreLock)
            {
                return _score;
            }
        }

        public void SetScore(MinimaxResult score)
        {
            lock (_scoreLock)
            {
                _score = score;
            }
        }

        public List<BoardSquare> FindPieces<TPiece>() where TPiece : GamePiece
        {
            var squares = new List<BoardSquare>();
            foreach (var square in _board.GetAllSquares())
            {
                if (square.GetPiece() is TPiece)
                {
                    squares.Add(square);
                }
            }

            return squares;
        }
    }
}
