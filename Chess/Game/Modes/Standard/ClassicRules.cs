using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class ClassicRules : IGameRules
    {
        public int CurrentPlayer { get; private set; }

        public int PlayerCount { get; } = 2;
        public int FileCount { get; } = 8;
        public int RankCount { get; } = 8;

        private StandardBoardEvaluator _evaluator;

        public ClassicRules()
        {
            _evaluator = new StandardBoardEvaluator(this);
        }

        public virtual BoardState Move(Move move)
        {
            StandardBoardState newBoardState;
            if(move is ClassicMove)
            {
                newBoardState = (move as ClassicMove).Process();
                move.BoardAfter = newBoardState;
                move.Notation = GetMoveNotation(move);
                return newBoardState;
            }
            newBoardState = ClassicMoveGenerator.GetMove(ConvertToStandardBoardState(move.BoardBefore), move).Process();
            move.BoardAfter = newBoardState;
            move.Piece.SetMoveCount(move.Piece.GetMoveCount() + 1);
            return newBoardState;
        }

        public virtual bool IsGameOver(BoardState state)
        {
            var standardBoardState = ConvertToStandardBoardState(state);
            int totalLegalMoves = 0;
            for(int i = 0; i < PlayerCount; i++)
            {
                int playerLegalMoves = ClassicMoveGenerator.GetAllLegalMoves(standardBoardState, i).Count;
                totalLegalMoves += playerLegalMoves;
                if(standardBoardState.IsInCheck(i) && playerLegalMoves == 0)
                {
                    return true;
                }
            }
            if(totalLegalMoves == 0)
            {
                return true;
            }
            return false;
        }

        public virtual GameResult GetGameResult(BoardState state)
        {
            if(!IsGameOver(state))
            {
                throw new Exception("Game is not over yet.");
            }

            return new GameResult(state.GetLastMove().Piece.GetPlayer());    
        }

        public virtual BoardState GetStartingBoardState()
        {
            var board = new Board(8, 8);
            for (int player = 0; player < PlayerCount; player++)
            {
                int firstRank = player == 0 ? 0 : 7;
                int secondRank = player == 0 ? 1 : 6;
                for (int file = 0; file < FileCount; file++)
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

        public virtual BoardState GetStartingBoardState(Board board)
        {
            return new StandardBoardState(board);
        }

        public virtual IEnumerable<Move> GetAllLegalMoves(BoardState currentState, int player)
        {
            return ClassicMoveGenerator.GetAllLegalMoves(ConvertToStandardBoardState(currentState), player);
        }

        public virtual IEnumerable<Move> GetLegalMoves(BoardSquare square, BoardState currentState)
        {
            return ClassicMoveGenerator.GetLegalMoves(ConvertToStandardBoardState(currentState), square);
        }

        public virtual string GetMoveNotation(Move move)
        {
            ClassicMove classicMove;
            if (move is not ClassicMove)
            {
                classicMove = ClassicMoveGenerator.GetMove(ConvertToStandardBoardState(move.BoardBefore), move);
            }
            else
            {
                classicMove = move as ClassicMove;
            }
            StringBuilder sb = new StringBuilder();
            int file = move.From.GetFile();
            if (classicMove is MoveCapture && move.Piece is Pawn)
            {
                sb.Append(file.ConvertToChessFile());
            }
            else
            {
                sb.Append(move.Piece.GetSymbol());
            }
            if (classicMove is MoveCapture)
            {
                sb.Append('x');
            }
            sb.Append(file.ConvertToChessFile());
            sb.Append(move.To.GetRank() + 1);
            if(ConvertToStandardBoardState(classicMove.BoardAfter).IsInCheck((move.Piece.GetPlayer() + 1) % PlayerCount))
            {
                sb.Append("+");
            }
            if(IsGameOver(move.BoardAfter))
            {
                sb.Append("#");
            }
            return sb.ToString();
        }

        public virtual IBoardEvaluator GetEvaluator()
        {
            return _evaluator;
        }

        private StandardBoardState ConvertToStandardBoardState(BoardState state)
        {
            if(state is StandardBoardState)
            {
                return (StandardBoardState)state;
            }
            else
            {
                return new StandardBoardState(state.GetBoard(), state.GetLastMove());
            }
        }
        
    }
}
