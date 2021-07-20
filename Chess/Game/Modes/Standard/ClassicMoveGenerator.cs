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
            ClassicMove move = null;

            if (MoveCastle.IsLegal(state, from, to))
            {
                move = new MoveCastle(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            else if (MovePromotion.IsLegal(state, from, to))
            {
                move = new MovePromotion(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            else if (MoveEnPassant.IsLegal(state, from, to))
            {
                move = new MoveEnPassant(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            else if(MoveCapture.IsLegal(from, to))
            {
                move = new MoveCapture(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }
            else if (MoveNormal.IsLegal(from, to))
            {
                move = new MoveNormal(rules)
                {
                    BoardBefore = state,
                    From = from,
                    To = to,
                    Piece = from.GetPiece()
                };
            }


            if (move != null && IsMovePreventedByCheck(state, move))
            {
                move = null;
            }

            return move;
        }

        public static ClassicMove GetMove(StandardBoardState state, Move move)
        {
            return GetMove(state, move.From, move.To);
        }

        private static bool IsMovePreventedByCheck(StandardBoardState state, ClassicMove move)
        {
            var newState = move.Process();
            return newState.IsInCheck(move.Piece.GetPlayer());
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
            return true;
        }
    }
}
