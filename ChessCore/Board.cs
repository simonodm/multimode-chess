using ChessCore.Exceptions;
using System.Collections.Generic;

namespace ChessCore
{
    /// <summary>
    /// Represents a game board.
    /// </summary>
    public class Board
    {
        private readonly BoardSquare[,] _board;
        private readonly int _width;
        private readonly int _height;

        public Board(int width, int height)
        {
            _board = new BoardSquare[width, height];
            _width = width;
            _height = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _board[i, j] = new BoardSquare(i, j);
                }
            }
        }

        private Board(BoardSquare[,] board)
        {
            _board = board;
            _width = _board.GetLength(0);
            _height = _board.GetLength(1);
        }

        /// <summary>
        /// Enumerates all squares on the board column by column from left to right.
        /// </summary>
        /// <returns>An enumerable of all board squares</returns>
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

        /// <summary>
        /// Retrieves the square at the specified file and rank.
        /// </summary>
        /// <param name="file">Square file</param>
        /// <param name="rank">Square rank</param>
        /// <returns>A board square</returns>
        public BoardSquare GetSquare(int file, int rank)
        {
            return _board[file, rank];
        }

        /// <summary>
        /// Moves the piece according to the supplied move, removing the piece at the target location.
        /// </summary>
        /// <param name="move"></param>
        /// <returns>Updated board</returns>
        public Board Move(Move move)
        {
            var newBoard = this;
            if (move.To.GetPiece() != null)
            {
                newBoard = RemovePiece(move.To);
            }
            newBoard = newBoard.AddPiece(move.To, move.Piece).RemovePiece(move.From);

            return newBoard;
        }

        /// <summary>
        /// Adds a piece to the square at the specified file and rank.
        /// </summary>
        /// <param name="file">Square file</param>
        /// <param name="rank">Square rank</param>
        /// <param name="piece">Piece to add</param>
        /// <returns>Updated board</returns>
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

        /// <summary>
        /// Adds a piece to the specified square
        /// </summary>
        /// <param name="square">Target square</param>
        /// <param name="piece">Piece to add</param>
        /// <returns>Updated board</returns>
        public Board AddPiece(BoardSquare square, GamePiece piece)
        {
            return AddPiece(square.GetFile(), square.GetRank(), piece);
        }

        /// <summary>
        /// Removes a piece from the square at the specified file and rank
        /// </summary>
        /// <param name="file">Square file</param>
        /// <param name="rank">Square rank</param>
        /// <returns>A new instance of Board with the piece removed.</returns>
        /// <exception cref="ChessCoreException">Thrown if there is no piece present on the board</exception>
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

        /// <summary>
        /// Removes a piece from the specified square.
        /// </summary>
        /// <param name="square">Square to remove the piece from</param>
        /// <returns>A new instance of Board with the piece removed.</returns>
        /// <exception cref="ChessCoreException">Thrown if there is no piece present on the board</exception>
        public Board RemovePiece(BoardSquare square)
        {
            return RemovePiece(square.GetFile(), square.GetRank());
        }

        /// <summary>
        /// Retrieves the board's width in squares.
        /// </summary>
        /// <returns>Board width</returns>
        public int GetWidth()
        {
            return _width;
        }

        /// <summary>
        /// Retrieves the board's height in squares.
        /// </summary>
        /// <returns>Board height</returns>
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
