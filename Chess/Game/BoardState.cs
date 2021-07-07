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
        private BoardSquare[,] _board = new BoardSquare[8, 8];
        
        public BoardState()
        {
            for(int i = 0; i < _board.GetLength(0); i++)
            {
                for(int j = 0; j < _board.GetLength(1); j++)
                {
                    _board[i, j] = new BoardSquare(i, j, null);
                }
            }
        }

        private BoardState(BoardSquare[,] board)
        {
            _board = board;
        }

        public IEnumerable<BoardSquare> GetAllSquares()
        {
            for(int i = 0; i < _board.GetLength(0); i++)
            {
                for(int j = 0; j < _board.GetLength(1); j++)
                {
                    yield return _board[i, j];
                }
            }
        }

        public BoardSquare GetSquare(int file, int rank)
        {
            return _board[file, rank];
        }

        public BoardState Move(Move move)
        {
            var newBoard = CopyBoard();
            newBoard[move.To.File, move.To.Rank].Piece = move.Piece;
            move.Piece.Square = newBoard[move.To.File, move.To.Rank];
            newBoard[move.From.File, move.From.Rank].Piece = null;
            var newBoardState = new BoardState(newBoard);
            return newBoardState;
        }

        public BoardState AddPiece(int file, int rank, IGamePiece piece)
        {
            if(_board[file, rank].Piece != null)
            {
                throw new Exception($"A piece is already present on square {rank}{file}.");
            }
            var newBoard = CopyBoard();
            newBoard[file, rank].Piece = piece;
            piece.Square = newBoard[file, rank];
            var newBoardState = new BoardState(newBoard);
            return newBoardState;
        }

        public BoardState AddPiece(BoardSquare square, IGamePiece piece)
        {
            return AddPiece(square.File, square.Rank, piece);
        }

        public BoardState RemovePiece(IGamePiece piece)
        {
            if(piece.Square == null)
            {
                return this;
            }
            return RemoveAt((BoardSquare)piece.Square);
        }

        public BoardState RemoveAt(int file, int rank)
        {
            if(_board[file, rank].Piece == null)
            {
                throw new Exception($"No piece found on square {rank}{file}");
            }
            var newBoard = CopyBoard();
            newBoard[file, rank].Piece.Square = null;
            newBoard[file, rank].Piece = null;
            
            var newBoardState = new BoardState(newBoard);
            return newBoardState;
        }

        public BoardState RemoveAt(BoardSquare square)
        {
            return RemoveAt(square.File, square.Rank);
        }

        public int GetBoardWidth()
        {
            return _board.GetLength(0);
        }

        public int GetBoardHeight()
        {
            return _board.GetLength(1);
        }

        public List<BoardSquare> FindPieces<TPiece>() where TPiece : IGamePiece
        {
            var squares = new List<BoardSquare>();
            foreach(var square in _board)
            {
                if(square.Piece is TPiece)
                {
                    squares.Add(square);
                }
            }
            return squares;
        }
        
        private BoardSquare[,] CopyBoard()
        {
            var newBoard = new BoardSquare[8, 8];
            for(int i = 0; i < _board.GetLength(0); i++)
            {
                for(int j = 0; j < _board.GetLength(1); j++)
                {
                    newBoard[i, j] = _board[i, j];
                }
            }
            return newBoard;
        }

    }
}
