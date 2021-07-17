using Chess.Game;
using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Board
    {
        private BoardSquare[,] _board;
        private int _width;
        private int _height;

        public Board(int width, int height)
        {
            _board = new BoardSquare[width, height];
            _width = width;
            _height = height;
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    _board[i, j] = new BoardSquare(i, j, null);
                }
            }
        }

        private Board(BoardSquare[,] board)
        {
            _board = board;
            _width = _board.GetLength(0);
            _height = _board.GetLength(1);
        }

        public IEnumerable<BoardSquare> GetAllSquares()
        {
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    yield return _board[i, j];
                }
            }
        }

        public BoardSquare GetSquare(int file, int rank)
        {
            return _board[file, rank];
        }

        public Board AddPiece(int file, int rank, IGamePiece piece)
        {
            if (_board[file, rank].Piece != null)
            {
                throw new Exception($"A piece is already present on square {file.ConvertToChessFile()}{rank + 1}.");
            }
            var newBoard = CopyBoard();
            newBoard[file, rank].Piece = piece;
            return new Board(newBoard);
        }

        public Board AddPiece(BoardSquare square, IGamePiece piece)
        {
            return AddPiece(square.File, square.Rank, piece);
        }

        public Board RemovePiece(int file, int rank)
        {
            var newBoard = CopyBoard();
            newBoard[file, rank].Piece.Square = null;
            newBoard[file, rank].Piece = null;
            return new Board(newBoard);
        }

        public Board RemovePiece(BoardSquare square)
        {
            return RemovePiece(square.File, square.Rank);
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        private BoardSquare[,] CopyBoard()
        {
            var newBoard = new BoardSquare[8, 8];
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    newBoard[i, j] = _board[i, j];
                }
            }
            return newBoard;
        }
    }
}
