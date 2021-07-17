using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
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
            MoveType type = GetMoveType(move);
            BoardState newBoardState;
            switch(type)
            {
                case MoveType.MOVE_NORMAL:
                    newBoardState = HandleNormal(move);
                    break;
                case MoveType.MOVE_CAPTURE:
                    newBoardState = HandleCapture(move);
                    break;
                case MoveType.MOVE_CASTLE:
                    newBoardState = HandleCastle(move);
                    break;
                case MoveType.MOVE_PROMOTION:
                    newBoardState = HandlePromotion(move);
                    break;
                case MoveType.MOVE_EN_PASSANT:
                    newBoardState = HandleEnPassant(move);
                    break;
                case MoveType.MOVE_ILLEGAL:
                default:
                    return move.BoardBefore;
            }

            move.Piece.SetMoveCount(move.Piece.GetMoveCount() + 1);
            move.BoardAfter = newBoardState;

            return newBoardState;
        }

        public virtual bool IsGameOver(BoardState state)
        {
            for(int i = 0; i < PlayerCount; i++)
            {
                if(IsCheck(state, i) && GetAllLegalMoves(state, i).Count == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual GameResult GetGameResult(BoardState state)
        {
            return new GameResult(state.GetLastMove().Piece.GetPlayer());    
        }

        public virtual BoardScore GetBoardScore(BoardState state)
        {

            return new BoardScore(0);
        }

        public virtual Board GetDefaultBoard()
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
            return board;
        }

        public virtual List<Move> GetAllLegalMoves(BoardState currentState, int player)
        {
            var moveList = new List<Move>();
            foreach (var square in currentState.GetBoard().GetAllSquares())
            {
                if (square.Piece != null && square.Piece.GetPlayer() == player)
                {
                    moveList.AddRange(GetLegalMoves(square, currentState));
                }
            }
            return moveList;
        }

        public virtual List<Move> GetLegalMoves(BoardSquare square, BoardState currentState)
        {
            return GetNonBlockedMoves(square, currentState).FindAll(move => GetMoveType(move) != MoveType.MOVE_ILLEGAL).ToList();
        }

        public virtual string GetMoveNotation(Move move)
        {
            StringBuilder sb = new StringBuilder();
            if (IsCapture(move) && move.Piece is Pawn)
            {
                sb.Append(move.From.File.ConvertToChessFile());
            }
            else
            {
                sb.Append(move.Piece.GetSymbol());
            }
            if (IsCapture(move))
            {
                sb.Append('x');
            }
            sb.Append(move.To.File.ConvertToChessFile());
            sb.Append(move.To.Rank + 1);
            if(IsCheck(move.BoardAfter, (move.Piece.GetPlayer() + 1) % PlayerCount))
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

        protected virtual List<Move> GetAllNonBlockedMoves(BoardState currentState)
        {
            var moves = new List<Move>();
            foreach(var square in currentState.GetBoard().GetAllSquares())
            {
                moves.AddRange(GetNonBlockedMoves(square, currentState));
            }
            return moves;
        }
        protected virtual List<Move> GetNonBlockedMoves(BoardSquare square, BoardState currentState)
        {
            var moves = new List<Move>();
            if(square.Piece == null)
            {
                return moves;
            }
            foreach((int, int) possibleMove in square.Piece.GetPossibleMoveOffsets())
            {
                int newFile = square.File + possibleMove.Item1;
                int newRank = square.Rank + possibleMove.Item2;
                if (!(newFile < 0 || newRank < 0 || newFile > 7 || newRank > 7))
                {
                    var move = new Move(this)
                    {
                        From = square,
                        To = currentState.GetBoard().GetSquare(newFile, newRank),
                        Piece = square.Piece,
                        BoardBefore = currentState
                    };
                    if (square.Piece is Pawn && (newRank == 0 || newRank == 7))
                    {
                        move.AddOption("Queen");
                        move.AddOption("Rook");
                        move.AddOption("Knight");
                        move.AddOption("Bishop");
                    }
                    
                    if(!IsMoveBlocked(move))
                    {
                        moves.Add(move);
                    }
                }
            }
            return moves;
        }

        protected virtual MoveType GetMoveType(Move move)
        {
            if(IsMoveBlocked(move) || IsPreventedByCheck(move))
            {
                return MoveType.MOVE_ILLEGAL;
            }
            if(IsPromotion(move))
            {
                return MoveType.MOVE_PROMOTION;
            }
            if(IsEnPassant(move))
            {
                return MoveType.MOVE_EN_PASSANT;
            }
            if(IsCapture(move))
            {
                return MoveType.MOVE_CAPTURE;
            }
            if (IsNormal(move))
            {
                return MoveType.MOVE_NORMAL;
            }
            if (IsCastle(move))
            {
                return MoveType.MOVE_CASTLE;
            }
            return MoveType.MOVE_ILLEGAL;
        }

        protected virtual BoardState HandlePromotion(Move move)
        {
            GamePiece piece;
            switch(move.SelectedOption.Id) // TODO: Move to piece factory
            {
                case 0:
                    piece = new Queen(move.Piece.GetPlayer());
                    break;
                case 1:
                    piece = new Rook(move.Piece.GetPlayer());
                    break;
                case 2:
                    piece = new Knight(move.Piece.GetPlayer());
                    break;
                case 3:
                    piece = new Bishop(move.Piece.GetPlayer());
                    break;
                default:
                    throw new Exception("Unrecognized option");
            }
            if(move.To.Piece != null)
            {
                if(move.To.Piece.GetPlayer() == move.Piece.GetPlayer())
                {
                    return move.BoardBefore;
                }
                var boardStateAfterCapture = HandleCapture(move);
                var boardAfterCapture = boardStateAfterCapture.GetBoard();
                if(boardAfterCapture.GetSquare(move.To.File, move.To.Rank).Piece == move.Piece)
                {
                    return new BoardState(boardAfterCapture.RemovePiece(move.To).AddPiece(move.To, piece), move);
                }
                else
                {
                    return boardStateAfterCapture;
                }
            }
            var newBoard = move.BoardBefore.GetBoard().AddPiece(move.To, piece).RemovePiece(move.From);
            return new BoardState(newBoard, move);
        }
        protected virtual BoardState HandleCastle(Move move)
        {
            var rookSquare = GetCastleRookSquare(move);

            var rookTargetSquare = move.To.File > move.From.File ?
                move.BoardBefore.GetBoard().GetSquare(move.From.File + 1, move.From.Rank) :
                move.BoardBefore.GetBoard().GetSquare(move.From.File - 1, move.From.Rank);

            var rookMove = new Move(this)
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.Piece
            };

            var newBoard = move.BoardBefore.GetBoard()
                .Move(move)
                .Move(rookMove);

            return new BoardState(newBoard, move);
        }

        protected virtual BoardState HandleCapture(Move move)
        {
            var newBoard = move.BoardBefore.GetBoard().Move(move);
            return new BoardState(newBoard, move);
        }

        protected virtual BoardState HandleNormal(Move move)
        {
            var newBoard = move.BoardBefore.GetBoard().Move(move);
            return new BoardState(newBoard, move);
        }

        protected virtual BoardState HandleEnPassant(Move move)
        {
            var enPassantSquare = move.BoardBefore.GetLastMove().To;
            var newBoard = move.BoardBefore.GetBoard()
                .RemovePiece(enPassantSquare)
                .Move(move);
            return new BoardState(newBoard);
        }

        private bool IsCapture(Move move)
        {
            if (IsEnPassant(move))
            {
                return true;
            }

            if (move.Piece is Pawn)
            {
                if(move.From.File == move.To.File ||
                   (move.Piece.GetPlayer() == 0 && move.To.Rank < move.From.Rank) ||
                   (move.Piece.GetPlayer() == 1 && move.To.Rank > move.From.Rank))
                {
                    return false;
                }
            }

            if(move.To.Piece != null && move.To.Piece.GetPlayer() != move.From.Piece.GetPlayer())
            {
                return true;
            }

            return false;
        }

        private bool IsEnPassant(Move move)
        {
            var previousMove = move.BoardBefore.GetLastMove();
            if(previousMove == null)
            {
                return false;
            }
            return move.Piece is Pawn &&
                previousMove.Piece is Pawn &&
                Math.Abs(previousMove.To.Rank - previousMove.From.Rank) == 2 &&
                move.To == GetEnPassantSquare(move);
        }

        private BoardSquare GetEnPassantSquare(Move move)
        {
            var previousMove = move.BoardBefore.GetLastMove();
            var enPassantFile = previousMove.From.File;
            var enPassantRank = previousMove.From.Rank + (previousMove.To.Rank - previousMove.From.Rank) / 2;
            var square = move.BoardBefore.GetBoard().GetSquare(enPassantFile, enPassantRank);
            return square;
        }

        private bool IsPromotion(Move move)
        {
            return move.SelectedOption != null;
        }

        private bool IsCastle(Move move)
        {
            if(CheckBaseCastleConditions(move))
            {
                for(int i = Math.Min(move.From.File, move.To.File); i <= Math.Max(move.From.File, move.To.File); i++)
                {
                    if(IsSquareUnderThreat(move.BoardBefore.GetBoard().GetSquare(i, move.From.Rank), move.BoardBefore, GetNextPlayer(move.Piece.GetPlayer())))
                    {
                        return false;
                    }
                }
                var expectedRook = GetCastleRookSquare(move).Piece;
                if(expectedRook is Rook && expectedRook.GetPlayer() == move.Piece.GetPlayer() && expectedRook.GetMoveCount() == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsNormal(Move move)
        {
            if(move.To.Piece == null)
            {
                if(move.Piece is King)
                {
                    return Math.Abs(move.From.Rank - move.To.Rank) <= 1 && Math.Abs(move.From.File - move.To.File) <= 1;
                }
                if(move.Piece is Pawn)
                {
                    if(move.To.File != move.From.File)
                    {
                        return false;
                    }
                    if(move.Piece.GetPlayer() == 0 && move.To.Rank - move.From.Rank < 0)
                    {
                        return false;
                    }
                    if(move.Piece.GetPlayer() == 1 && move.From.Rank - move.To.Rank < 0)
                    {
                        return false;
                    }
                    if(move.To.Rank - move.From.Rank == 2)
                    {
                        return move.From.Rank == 1;
                    }
                    if(move.From.Rank - move.To.Rank == 2)
                    {
                        return move.From.Rank == 6;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckBaseCastleConditions(Move move)
        {
            return move.Piece is King &&
               Math.Abs(move.To.File - move.From.File) == 2 &&
               move.From.File == 4 &&
               move.Piece.GetMoveCount() == 0 &&
               (move.From.Rank == 0 || move.From.Rank == 7);
        }

        private BoardSquare GetCastleRookSquare(Move move)
        {
            return move.To.File < move.From.File ?
                    move.BoardBefore.GetBoard().GetSquare(0, move.From.Rank) :
                    move.BoardBefore.GetBoard().GetSquare(7, move.From.Rank);
        }

        private bool IsSquareUnderThreat(BoardSquare square, BoardState state, int byPlayer)
        {
            var moves = GetAllNonBlockedMoves(state);

            foreach(var move in moves)
            {
                if(move.To == square &&
                   move.Piece.GetPlayer() == byPlayer)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsCheck(BoardState state, int player)
        {
            var kingSquares = state.FindPieces<King>();

            foreach(var square in kingSquares)
            {
                if(square.Piece.GetPlayer() == player && IsSquareUnderThreat(square, state, GetNextPlayer(square.Piece.GetPlayer()))) {
                    return true;
                }
            }

            return false;
        }

        private bool IsMoveBlocked(Move move)
        {
            if(move.Piece is Knight)
            {
                return false;
            }

            bool pieceFound = false;
            int startFile = move.From.File;
            int endFile = move.To.File;
            int startRank = move.From.Rank;
            int endRank = move.To.Rank;
            int i = startFile;
            int j = startRank;

            while(i != endFile || j != endRank)
            {
                var piece = move.BoardBefore.GetBoard().GetSquare(i, j).Piece;
                if(piece != null && piece != move.Piece)
                {
                    pieceFound = true;
                }
                if (pieceFound)
                {
                    return true;
                }
                if (startFile < endFile && i < endFile) i++;
                if (startFile > endFile && i > endFile) i--;
                if (startRank < endRank && j < endRank) j++;
                if (startRank > endRank && j > endRank) j--;
            }

            if(move.To.Piece != null && move.To.Piece.GetPlayer() == move.From.Piece.GetPlayer())
            {
                return true;
            }

            return false;
        }
        private bool IsPreventedByCheck(Move move)
        {
            var newBoard = move.BoardBefore.GetBoard().Move(move);
            var newState = new BoardState(newBoard);
            return IsCheck(newState, move.Piece.GetPlayer());
        }
    
        private int GetNextPlayer(int player)
        {
            return (player + 1) % PlayerCount;
        }
    }
}
