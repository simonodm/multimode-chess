using ChessCore.Exceptions;
using ChessCore.Modes.Standard.Pieces;
using System.Collections.Generic;
using System.Text;

namespace ChessCore.Modes.Standard
{
    public class StandardRules : IGameRules
    {
        private StandardPieceFactory _pieceFactory;
        private StandardBoardEvaluator _evaluator;

        public virtual BoardState Move(Move move)
        {
            if (move is not StandardMove)
            {
                throw new InvalidMoveException("Invalid move supplied.");
            }
            return (move as StandardMove).Process();
        }

        public IReadOnlyList<Move> GetAllLegalMoves(BoardState state, int player)
        {
            List<Move> moves = new List<Move>();
            foreach (var square in state.GetBoard().GetAllSquares())
            {
                moves.AddRange(GetLegalMoves(square, state, player));
            }
            return moves;
        }

        public IReadOnlyList<Move> GetLegalMoves(BoardSquare square, BoardState state, int player)
        {
            if (square.GetPiece() == null || square.GetPiece().GetPlayer() != player || square.GetPiece() is not StandardPiece)
            {
                return new List<Move>();
            }

            return (square.GetPiece() as StandardPiece).GetLegalMoves(state.ToStandardBoardState(), square);
        }

        public virtual string GetMoveNotation(Move move)
        {
            StringBuilder sb = new StringBuilder();

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

            if (move.BoardAfter != null)
            {
                var gameResult = GetGameResult(move.BoardAfter);
                if (gameResult == GameResult.WHITE_WIN || gameResult == GameResult.BLACK_WIN)
                {
                    sb.Append("#");
                }
                else if (move.BoardAfter.ToStandardBoardState().IsInCheck((move.Piece.GetPlayer() + 1) % 2))
                {
                    sb.Append("+");
                }
            }

            return sb.ToString();
        }

        public IBoardEvaluator GetEvaluator()
        {
            if (_evaluator == null)
            {
                _evaluator = new StandardBoardEvaluator(this);
            }
            return _evaluator;
        }

        public virtual GameResult GetGameResult(BoardState state)
        {
            var standardBoardState = state.ToStandardBoardState();
            for (int player = 0; player < 2; player++)
            {
                int playerLegalMoves = GetAllLegalMoves(standardBoardState, player).Count;
                if (playerLegalMoves == 0)
                {
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
                    else
                    {
                        return GameResult.DRAW;
                    }
                }
            }
            return GameResult.ONGOING;
        }

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
                    if (file == 0 || file == 7)
                    {
                        piece = new Rook(player);
                    }
                    if (file == 1 || file == 6)
                    {
                        piece = new Knight(player);
                    }
                    if (file == 2 || file == 5)
                    {
                        piece = new Bishop(player);
                    }
                    if (file == 3)
                    {
                        piece = new Queen(player);
                    }
                    if (file == 4)
                    {
                        piece = new King(player);
                    }
                    board = board.AddPiece(squareFirstRank, piece);
                    board = board.AddPiece(squareSecondRank, new Pawn(player));
                }
            }

            return new StandardBoardState(board);
        }

        public IPieceFactory GetPieceFactory()
        {
            if (_pieceFactory == null)
            {
                _pieceFactory = new StandardPieceFactory();
            }
            return _pieceFactory;
        }

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

    }
}
