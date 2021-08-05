using ChessCore.Exceptions;
using System.Collections.Generic;

namespace ChessCore
{
    public class Board
    {
        private BoardSquare[,] _board;
        private int _width;
        private int _height;

        public Board(int width, int height)
        {
            _board = new BoardSquare[width, height];
            _width = width;
            _height = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
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
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    yield return _board[i, j];
                }
            }
        }

        public BoardSquare GetSquare(int file, int rank)
        {
            return _board[file, rank];
        }

        public Board Move(Move move)
        {
            Board newBoard = this;
            if (move.To.GetPiece() != null)
            {
                newBoard = RemovePiece(move.To);
            }
            newBoard = newBoard.AddPiece(move.To, move.Piece).RemovePiece(move.From);
            return newBoard;
        }

        public Board AddPiece(int file, int rank, GamePiece piece)
        {
            if (_board[file, rank].GetPiece() != null)
            {
                throw new ChessCoreException($"A piece is already present on square {file.ConvertToChessFile()}{rank + 1}.");
            }
            var newBoard = CopyBoard();
            newBoard[file, rank].SetPiece(piece);
            return new Board(newBoard);
        }

        public Board AddPiece(BoardSquare square, GamePiece piece)
        {
            return AddPiece(square.GetFile(), square.GetRank(), piece);
        }

        public Board RemovePiece(int file, int rank)
        {
            if (_board[file, rank].GetPiece() == null)
            {
                throw new ChessCoreException($"No piece found on square {file.ConvertToChessFile()}{rank + 1}");
            }
            var newBoard = CopyBoard();
            newBoard[file, rank].SetPiece(null);
            return new Board(newBoard);
        }

        public Board RemovePiece(BoardSquare square)
        {
            return RemovePiece(square.GetFile(), square.GetRank());
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
            var newBoard = new BoardSquare[_width, _height];
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    newBoard[i, j] = _board[i, j];
                }
            }
            return newBoard;
        }
    }
}
