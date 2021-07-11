using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class ClassicRules : IGameRules
    {
        public int CurrentPlayer { get; private set; }

        private const int PLAYER_COUNT = 2;
        private const int FILE_COUNT = 8;
        private const int RANK_COUNT = 8;

        private BoardSquare _enPassantSquare;
        private bool _enPassantPossible = false;
        private IGamePiece _enPassantPiece;

        private bool _gameOver = false;
        private GameResult _gameResult = null;

        public virtual BoardState Move(Move move)
        {
            MoveType type = GetMoveType(move);
            BoardState newBoardState;
            switch(type)
            {
                case MoveType.MOVE_NORMAL:
                    _enPassantPossible = false;
                    newBoardState = HandleNormal(move);
                    break;
                case MoveType.MOVE_CAPTURE:
                    newBoardState = HandleCapture(move);
                    _enPassantPossible = false;
                    break;
                case MoveType.MOVE_CASTLE:
                    newBoardState = HandleCastle(move);
                    _enPassantPossible = false;
                    break;
                case MoveType.MOVE_PROMOTION:
                    newBoardState = HandlePromotion(move);
                    _enPassantPossible = false;
                    break;
                case MoveType.MOVE_EN_PASSANT:
                    newBoardState = HandleEnPassant(move);
                    _enPassantPossible = false;
                    break;
                case MoveType.MOVE_ILLEGAL:
                default:
                    return move.BoardBefore;
            }

            CurrentPlayer = (CurrentPlayer + 1) % PLAYER_COUNT;

            if (IsGameOver(newBoardState))
            {
                _gameOver = true;
                _gameResult = new GameResult((CurrentPlayer+1)%PLAYER_COUNT);
            }

            move.Piece.MoveCount++;
            move.BoardAfter = newBoardState;

            return newBoardState;
        }

        public virtual bool IsGameOver(BoardState state)
        {
            if(_gameOver == false)
            {
                for (int i = 0; i < PLAYER_COUNT; i++)
                {
                    if (IsCheck(state, i) && GetAllLegalMoves(state, i).Count == 0)
                    {
                        _gameOver = true;
                    }
                }
            }
            return _gameOver;
        }

        public virtual GameResult GetGameResult()
        {
            return _gameResult;    
        }

        public virtual BoardScore GetBoardScore(BoardState state)
        {
            return new BoardScore(0);
        }

        public virtual BoardState GetDefaultBoard()
        {
            var state = new BoardState();
            for (int player = 0; player < PLAYER_COUNT; player++)
            {
                int firstRank = player == 0 ? 0 : 7;
                int secondRank = player == 0 ? 1 : 6;
                for (int file = 0; file < FILE_COUNT; file++)
                {
                    var squareFirstRank = state.GetSquare(file, firstRank);
                    var squareSecondRank = state.GetSquare(file, secondRank);
                    IGamePiece piece = null;
                    if (file == 0 || file == 7)
                    {
                        piece = new Rook
                        {
                            Player = player
                        };
                    }
                    if (file == 1 || file == 6)
                    {
                        piece = new Knight
                        {
                            Player = player
                        };
                    }
                    if (file == 2 || file == 5)
                    {
                        piece = new Bishop
                        {
                            Player = player
                        };
                    }
                    if (file == 3)
                    {
                        piece = new Queen
                        {
                            Player = player
                        };
                    }
                    if (file == 4)
                    {
                        piece = new King
                        {
                            Player = player
                        };
                    }
                    state = state.AddPiece(squareFirstRank, piece);
                    state = state.AddPiece(squareSecondRank, new Pawn
                    {
                        Player = player
                    });
                }
            }
            return state;
        }

        public virtual List<Move> GetAllLegalMoves(BoardState currentState, int player)
        {
            var moveList = new List<Move>();
            foreach (var square in currentState.GetAllSquares())
            {
                if (square.Piece != null && square.Piece.Player == player)
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
                sb.Append(GetPieceSymbol(move.Piece));
            }
            if (IsCapture(move))
            {
                sb.Append('x');
            }
            sb.Append(move.To.File.ConvertToChessFile());
            sb.Append(move.To.Rank + 1);
            if(IsCheck(move.BoardAfter, (move.Piece.Player + 1) % PLAYER_COUNT))
            {
                sb.Append("+");
            }
            if(IsGameOver(move.BoardAfter))
            {
                sb.Append("#");
            }
            return sb.ToString();
        }

        protected virtual List<Move> GetAllNonBlockedMoves(BoardState currentState)
        {
            var moves = new List<Move>();
            foreach(var square in currentState.GetAllSquares())
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
            foreach((int, int) possibleMove in square.Piece.PossibleMoveOffsets)
            {
                int newFile = square.File + possibleMove.Item1;
                int newRank = square.Rank + possibleMove.Item2;
                if (!(newFile < 0 || newRank < 0 || newFile > 7 || newRank > 7))
                {
                    var move = new Move()
                    {
                        From = square,
                        To = currentState.GetSquare(newFile, newRank),
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
            if(IsCastle(move))
            {
                return MoveType.MOVE_CASTLE;
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
            return MoveType.MOVE_ILLEGAL;
        }

        protected virtual BoardState HandlePromotion(Move move)
        {
            IGamePiece piece;
            switch(move.SelectedOption.Id)
            {
                case 0:
                    piece = new Queen
                    {
                        Square = move.To,
                        Player = move.Piece.Player
                    };
                    break;
                case 1:
                    piece = new Rook
                    {
                        Square = move.To,
                        Player = move.Piece.Player
                    };
                    break;
                case 2:
                    piece = new Knight
                    {
                        Square = move.To,
                        Player = move.Piece.Player
                    };
                    break;
                case 3:
                    piece = new Bishop
                    {
                        Square = move.To,
                        Player = move.Piece.Player
                    };
                    break;
                default:
                    throw new Exception("Unrecognized option");
            }
            if(move.To.Piece != null)
            {
                if(move.To.Piece.Player == move.Piece.Player)
                {
                    return move.BoardBefore;
                }
                HandleCapture(move);
                if(move.To.Piece == move.Piece)
                {
                    return move.BoardBefore.RemoveAt(move.To).AddPiece(move.To, piece);
                }
            }
            return move.BoardBefore.AddPiece(move.To, piece).RemoveAt(move.From);
        }
        protected virtual BoardState HandleCastle(Move move)
        {
            var rookSquare = GetCastleRookSquare(move);

            var rookTargetSquare = move.To.File > move.From.File ?
                move.BoardBefore.GetSquare(move.From.File + 1, move.From.Rank) :
                move.BoardBefore.GetSquare(move.From.File - 1, move.From.Rank);

            var rookMove = new Move
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.Piece
            };

            return move.BoardBefore.Move(move).Move(rookMove);
        }

        protected virtual BoardState HandleCapture(Move move)
        {
            return move.BoardBefore.RemoveAt(move.To).Move(move);
        }

        protected virtual BoardState HandleNormal(Move move)
        {
            BoardState newBoardState = move.BoardBefore.Move(move);

            if(move.Piece is Pawn && Math.Abs(move.To.Rank - move.From.Rank) == 2)
            {
                int enPassantSquareRank = move.From.Rank + (move.To.Rank - move.From.Rank) / 2;
                _enPassantSquare = newBoardState.GetSquare(move.To.File, enPassantSquareRank);
                _enPassantPossible = true;
                _enPassantPiece = move.Piece;
            }

            return newBoardState;
        }
        
        protected virtual BoardState HandleEnPassant(Move move)
        {
            return move.BoardBefore.Move(move).RemovePiece(_enPassantPiece);
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
                   (move.Piece.Player == 0 && move.To.Rank < move.From.Rank) ||
                   (move.Piece.Player == 1 && move.To.Rank > move.From.Rank))
                {
                    return false;
                }
            }

            if(move.To.Piece != null && move.To.Piece.Player != move.From.Piece.Player)
            {
                return true;
            }

            return false;
        }

        private bool IsEnPassant(Move move)
        {
            return move.Piece is Pawn &&
                _enPassantPossible &&
                _enPassantPiece.Player != move.Piece.Player &&
                move.To == _enPassantSquare;
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
                    if(IsSquareUnderThreat(move.BoardBefore.GetSquare(i, move.From.Rank), move.BoardBefore))
                    {
                        return false;
                    }
                }
                var expectedRook = GetCastleRookSquare(move).Piece;
                if(expectedRook is Rook && expectedRook.Player == move.Piece.Player && expectedRook.MoveCount == 0)
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
                    if(move.Piece.Player == 0 && move.To.Rank - move.From.Rank < 0)
                    {
                        return false;
                    }
                    if(move.Piece.Player == 1 && move.From.Rank - move.To.Rank < 0)
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
               move.Piece.MoveCount == 0 &&
               (move.From.Rank == 0 || move.From.Rank == 7);
        }

        private BoardSquare GetCastleRookSquare(Move move)
        {
            return move.To.File < move.From.File ?
                    move.BoardBefore.GetSquare(0, move.From.Rank) :
                    move.BoardBefore.GetSquare(7, move.From.Rank);
        }

        private bool IsSquareUnderThreat(BoardSquare square, BoardState state)
        {
            var moves = GetAllNonBlockedMoves(state);

            foreach(var move in moves)
            {
                if(move.To == square &&
                   ((square.Piece != null && move.Piece.Player != square.Piece.Player) ||
                   (square.Piece == null && move.Piece.Player != CurrentPlayer)))
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
                if(square.Piece.Player == player && IsSquareUnderThreat(square, state)) {
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
                var piece = move.BoardBefore.GetSquare(i, j).Piece;
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

            if(move.To.Piece != null && move.To.Piece.Player == move.From.Piece.Player)
            {
                return true;
            }

            return false;
        }
        private bool IsPreventedByCheck(Move move)
        {
            return IsCheck(move.BoardBefore.Move(move), move.Piece.Player);
        }
    
        private char GetPieceSymbol(IGamePiece piece)
        {
            char symbol = ' ';
            if (piece is Knight)
            {
                symbol = 'N';
            }
            else if (piece is Bishop)
            {
                symbol = 'B';
            }
            else if (piece is Rook)
            {
                symbol = 'R';
            }
            else if (piece is Queen)
            {
                symbol = 'Q';
            }
            else if (piece is King)
            {
                symbol = 'K';
            }
            return symbol;
        }
    }
}
