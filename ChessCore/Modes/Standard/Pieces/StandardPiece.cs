using System.Collections.Generic;

namespace ChessCore.Game.Modes.Standard
{
    public abstract class StandardPiece : GamePiece
    {
        protected HashSet<(int, int)> PossibleMoves;

        public StandardPiece(int player) : base(player) { }
        public virtual List<StandardMove> GetLegalMoves(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = GetThreatenedSquares(state, from);
            var legalMoves = new List<StandardMove>();
            foreach(var square in threatenedSquares)
            {
                var move = GenerateMove(state, from, square);
                if(move != null && !move.Process().IsInCheck(GetPlayer()))
                {
                    legalMoves.Add(move);
                }
            }
            return legalMoves;
        }

        public virtual List<BoardSquare> GetThreatenedSquares(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = new List<BoardSquare>();
            foreach(var possibleMove in PossibleMoves)
            {
                if(!IsOutOfBounds(possibleMove, state, from))
                {
                    var squareTo = GetTargetSquare(possibleMove, state, from);
                    if(!state.IsLineBlocked(from, squareTo))
                    {
                        threatenedSquares.Add(squareTo);
                    }
                }
            }
            return threatenedSquares;
        }

        protected bool IsOutOfBounds((int offsetFile, int offsetRank) possibleMove, BoardState state, BoardSquare from)
        {
            int newFile = from.GetFile() + possibleMove.offsetFile;
            int newRank = from.GetRank() + possibleMove.offsetRank;
            return newFile < 0 || newFile >= state.GetBoard().GetWidth() || newRank < 0 || newRank >= state.GetBoard().GetHeight();
        }

        protected virtual StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            StandardMove move = null;
            if (!state.IsLineBlocked(from, to))
            {
                if (to.GetPiece() != null && to.GetPiece().GetPlayer() != GetPlayer())
                {
                    move = new MoveCapture()
                    {
                        Piece = this,
                        From = from,
                        To = to,
                        BoardBefore = state
                    };
                }
                else if (to.GetPiece() == null)
                {
                    move = new MoveNormal()
                    {
                        Piece = this,
                        From = from,
                        To = to,
                        BoardBefore = state
                    };
                }
            }
            return move;
        }

        protected BoardSquare GetTargetSquare((int offsetFile, int offsetRank) possibleMove, BoardState state, BoardSquare from)
        {
            int newFile = from.GetFile() + possibleMove.offsetFile;
            int newRank = from.GetRank() + possibleMove.offsetRank;
            return state.GetBoard().GetSquare(newFile, newRank);
        }
    }
}
