using ChessCore.Exceptions;
using ChessCore.Modes.Standard.Pieces;
using System.Collections.Generic;
using System.Text;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// Represents the standard chess game mode.
    /// </summary>
    public class StandardRules : IGameRules
    {
        private StandardPieceFactory _pieceFactory;
        private StandardBoardEvaluator _evaluator;

        /// <inheritdoc cref="IGameRules.Move"/>
        /// <exception cref="InvalidMoveException">If an illegal move is supplied</exception>
        public virtual BoardState Move(Move move)
        {
            if (move is not StandardMove)
            {
                throw new InvalidMoveException("Invalid move supplied.");
            }
            move.Piece.SetMoveCount(move.Piece.GetMoveCount() + 1);
            return ((StandardMove)move).Process();
        }

        /// <inheritdoc cref="IGameRules.GetAllLegalMoves"/>
        public IReadOnlyList<Move> GetAllLegalMoves(BoardState state, int player)
        {
            var moves = new List<Move>();
            foreach (var square in state.GetBoard().GetAllSquares())
            {
                moves.AddRange(GetLegalMoves(square, state, player));
            }
            return moves;
        }

        /// <inheritdoc cref="IGameRules.GetLegalMoves"/>
        public IReadOnlyList<Move> GetLegalMoves(BoardSquare square, BoardState state, int player)
        {
            if (square.GetPiece() == null || square.GetPiece().GetPlayer() != player || square.GetPiece() is not StandardPiece)
            {
                return new List<Move>();
            }

            return ((StandardPiece)square.GetPiece()).GetLegalMoves(state.ToStandardBoardState(), square);
        }

        /// <inheritdoc cref="IGameRules.GetMoveNotation"/>
        public virtual string GetMoveNotation(Move move)
        {
            var sb = new StringBuilder();

            int fileFrom = move.From.GetFile();
            int fileTo = move.To.GetFile();

            if (move is MoveCapture && move.Piece is Pawn)
            {
                sb.Append(fileFrom.ConvertToChessFile());
            }
            else
            {
                sb.Append(move.Piece.GetSymbol());
            }

            if (move is MoveCapture)
            {
                sb.Append('x');
            }

            sb.Append(fileTo.ConvertToChessFile());
            sb.Append(move.To.GetRank() + 1);

            if (move.BoardAfter == null) return sb.ToString();

            var gameResult = GetGameResult(move.BoardAfter);
            if (gameResult == GameResult.WHITE_WIN || gameResult == GameResult.BLACK_WIN)
            {
                sb.Append("#");
            }
            else if (move.BoardAfter.ToStandardBoardState().IsInCheck((move.Piece.GetPlayer() + 1) % 2))
            {
                sb.Append("+");
            }

            return sb.ToString();
        }

        /// <inheritdoc cref="IGameRules.GetEvaluator"/>
        public IBoardEvaluator GetEvaluator()
        {
            return _evaluator ??= new StandardBoardEvaluator(this);
        }

        /// <inheritdoc cref="IGameRules.GetGameResult"/>
        public virtual GameResult GetGameResult(BoardState state)
        {
            var standardBoardState = state.ToStandardBoardState();
            for (int player = 0; player < 2; player++)
            {
                int playerLegalMoves = GetAllLegalMoves(standardBoardState, player).Count;
                if (playerLegalMoves != 0) continue;

                if (standardBoardState.IsInCheck(player))
                {
                    if (player == 0)
                    {
                        return GameResult.BLACK_WIN;
                    }
                    else
                    {
                        return GameResult.WHITE_WIN;
                    }
                }
                if (player == GetCurrentPlayer(state))
                {
                    return GameResult.DRAW;
                }
            }
            return GameResult.ONGOING;
        }

        /// <inheritdoc cref="IGameRules.GetStartingBoardState()"/>
        public virtual BoardState GetStartingBoardState()
        {
            var board = new Board(8, 8);
            for (int player = 0; player < 2; player++)
            {
                int firstRank = player == 0 ? 0 : 7;
                int secondRank = player == 0 ? 1 : 6;
                for (int file = 0; file < 8; file++)
                {
                    var squareFirstRank = board.GetSquare(file, firstRank);
                    var squareSecondRank = board.GetSquare(file, secondRank);

                    GamePiece piece = null;
                    switch (file)
                    {
                        case 0:
                        case 7:
                            piece = new Rook(player);
                            break;
                        case 1:
                        case 6:
                            piece = new Knight(player);
                            break;
                        case 2:
                        case 5:
                            piece = new Bishop(player);
                            break;
                        case 3:
                            piece = new Queen(player);
                            break;
                        case 4:
                            piece = new King(player);
                            break;
                    }

                    board = board.AddPiece(squareFirstRank, piece);
                    board = board.AddPiece(squareSecondRank, new Pawn(player));
                }
            }

            return new StandardBoardState(board);
        }

        /// <inheritdoc cref="IGameRules.GetPieceFactory"/>
        public IPieceFactory GetPieceFactory()
        {
            return _pieceFactory ??= new StandardPieceFactory();
        }

        /// <inheritdoc cref="IGameRules.GetStartingBoardState(Board)"/>
        public BoardState GetStartingBoardState(Board board)
        {
            var boardState = new StandardBoardState(board);
            var kings = boardState.FindPieces<King>();
            int whiteKings = 0;
            int blackKings = 0;
            foreach (var kingSquare in kings)
            {
                if (kingSquare.GetPiece().GetPlayer() == 0)
                {
                    whiteKings++;
                }
                else if (kingSquare.GetPiece().GetPlayer() == 1)
                {
                    blackKings++;
                }
                else
                {
                    throw new InvalidBoardException("Invalid king player found");
                }
            }
            if (whiteKings != 1 || blackKings != 1)
            {
                throw new InvalidBoardException("Invalid king count");
            }

            return new StandardBoardState(board);
        }

        private int GetCurrentPlayer(BoardState state)
        {
            if (state.GetLastMove() == null)
            {
                return 0;
            }

            return (state.GetLastMove().Piece.GetPlayer() + 1) % 2;
        }
    }
}
