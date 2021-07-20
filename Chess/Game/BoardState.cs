using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class BoardState
    {
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
            return _score;
        }

        public void SetScore(MinimaxResult score)
        {
            _score = score;
        }

        public List<BoardSquare> FindPieces<TPiece>() where TPiece : GamePiece
        {
            var squares = new List<BoardSquare>();
            foreach(var square in _board.GetAllSquares())
            {
                if(square.GetPiece() is TPiece)
                {
                    squares.Add(square);
                }
            }
            return squares;
        }

        public long GetHash()
        {
            // TODO: better hashing function
            long hash = 0;
            foreach(var square in _board.GetAllSquares())
            {
                int linearPosition = square.GetFile() * GetBoard().GetWidth() + square.GetRank();
                if(square.GetPiece() != null)
                {
                    var squareHash = square.GetPiece().GetValue() * linearPosition;
                    if(square.GetPiece().GetPlayer() == 1)
                    {
                        squareHash *= -1;
                    }
                    hash += squareHash;
                }
            }
            return hash;
        }

    }
}
