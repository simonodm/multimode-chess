using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    static class ClassicMoveGenerator
    {
        public static List<ClassicMove> GetAllLegalMoves(StandardBoardState state, int player)
        {
            var moveList = new List<ClassicMove>();
            foreach(var square in state.GetBoard().GetAllSquares())
            {
                if(square.GetPiece() != null && square.GetPiece().GetPlayer() == player)
                {
                    moveList.AddRange(GetLegalMoves(state, square));
                }
            }
            return moveList;
        }

        public static List<ClassicMove> GetLegalMoves(StandardBoardState state, BoardSquare square)
        {
            var moveList = new List<ClassicMove>();
            return state.GetNonBlockedMoves(square).Select(move => GetMove(state, move)).Where(move => move != null).ToList();
        }

        public static ClassicMove GetMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if(!CheckBasePawnConditions(state, from, to))
            {
                return null;
            }

            var rules = GameModePool.Get<ClassicRules>();
            if(IsNormal(from, to))
            {
                return new MoveNormal(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            if(IsCapture(from, to))
            {
                return new MoveCapture(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            if(IsEnPassant(state, from, to))
            {
                return new MoveEnPassant(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            if(IsPromotion(state, from, to))
            {
                return new MovePromotion(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            if(IsCastle(state, from, to))
            {
                return new MoveCastle(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            return null;
        }

        public static ClassicMove GetMove(StandardBoardState state, Move move)
        {
            return GetMove(state, move.From, move.To);
        }

        private static bool IsNormal(BoardSquare from, BoardSquare to)
        {
            if(to.GetPiece() == null)
            {
                var movePiece = from.GetPiece();
                if(movePiece is King)
                {
                    return Math.Abs(from.GetRank() - to.GetRank()) == 1;
                }
                if(movePiece is Pawn)
                {
                    if (to.GetFile() != from.GetFile())
                    {
                        return false;
                    }
                    if (movePiece.GetPlayer() == 0 && to.GetRank() - from.GetRank() < 0)
                    {
                        return false;
                    }
                    if (movePiece.GetPlayer() == 1 && from.GetRank() - to.GetRank() < 0)
                    {
                        return false;
                    }
                    if (to.GetRank() - from.GetRank() == 2)
                    {
                        return from.GetRank() == 1;
                    }
                    if (from.GetRank() - to.GetRank() == 2)
                    {
                        return from.GetRank() == 6;
                    }
                    return true;
                }
                return true;
            }
            return false;
        }

        private static bool IsCapture(BoardSquare from, BoardSquare to)
        {
            var movePiece = from.GetPiece();
            if(movePiece is Pawn)
            {
                if (from.GetFile() == to.GetFile() ||
                   (movePiece.GetPlayer() == 0 && to.GetRank() < from.GetRank()) ||
                   (movePiece.GetPlayer() == 1 && to.GetRank() > from.GetRank()))
                {
                    return false;
                }
            }

            if(to.GetPiece() != null && to.GetPiece().GetPlayer() != from.GetPiece().GetPlayer())
            {
                return true;
            }

            return false;
        }

        private static bool IsEnPassant(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            var previousMove = state.GetLastMove();
            if(previousMove == null)
            {
                return false;
            }
            return from.GetPiece() is Pawn &&
                previousMove.Piece is Pawn &&
                Math.Abs(previousMove.To.GetRank() - previousMove.From.GetRank()) == 2 &&
                to == GetEnPassantSquare(state);
        }

        private static bool IsCastle(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (CheckBaseCastleConditions(from, to))
            {
                for (int i = Math.Min(from.GetFile(), to.GetFile()); i <= Math.Max(from.GetFile(), to.GetFile()); i++)
                {
                    var square = state.GetBoard().GetSquare(i, from.GetRank());
                    if(state.IsSquareUnderThreat(square, (from.GetPiece().GetPlayer() + 1) % 2))
                    {
                        return false;
                    }
                }
                var expectedRook = GetCastleRookSquare(state, from, to).GetPiece();
                if (expectedRook is Rook && expectedRook.GetPlayer() == from.GetPiece().GetPlayer() && expectedRook.GetMoveCount() == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckBaseCastleConditions(BoardSquare from, BoardSquare to)
        {
            var movePiece = from.GetPiece();
            return movePiece is King &&
               Math.Abs(to.GetFile() - from.GetFile()) == 2 &&
               from.GetFile() == 4 &&
               movePiece.GetMoveCount() == 0 &&
               (from.GetRank() == 0 || from.GetRank() == 7);
        }

        private static BoardSquare GetCastleRookSquare(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            return to.GetFile() < from.GetFile() ?
                    state.GetBoard().GetSquare(0, from.GetRank()) :
                    state.GetBoard().GetSquare(7, from.GetRank());
        }

        private static BoardSquare GetEnPassantSquare(StandardBoardState state)
        {
            var previousMove = state.GetLastMove();
            var enPassantFile = previousMove.From.GetFile();
            var enPassantRank = previousMove.From.GetRank() + (previousMove.To.GetRank() - previousMove.From.GetRank()) / 2;
            var square = state.GetBoard().GetSquare(enPassantFile, enPassantRank);
            return square;
        }

        private static bool IsPromotion(StandardBoardState state, BoardSquare from, BoardSquare to)
        {

            return from.GetPiece() is Pawn && (to.GetRank() == 0 || to.GetRank() == state.GetBoard().GetHeight() - 1);
        }

        private static bool CheckBasePawnConditions(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            var piece = from.GetPiece();
            if(piece is not Pawn)
            {
                return true;
            }
            
            if(piece.GetPlayer() == 0)
            {
                if(to.GetRank() <= from.GetRank())
                {
                    return false;
                }
            }
            else
            {
                if(to.GetRank() >= from.GetRank())
                {
                    return false;
                }
            }
            if (to.GetFile() != from.GetFile())
            {
                return to.GetPiece() != null && to.GetPiece().GetPlayer() != piece.GetPlayer();
            }
            return true;
        }
    }
}
