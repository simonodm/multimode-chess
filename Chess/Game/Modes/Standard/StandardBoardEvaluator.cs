using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Modes;

namespace Chess.Game.Modes.Standard
{
    class StandardBoardEvaluator : IBoardEvaluator
    {
        private IGameRules _rules;

        public StandardBoardEvaluator(IGameRules rules)
        {
            _rules = rules;
        }

        public double GetBoardScore(BoardState state)
        {
            double score = 0;
            var convertedState = state.ToStandardBoardState();
            if(_rules.IsGameOver(convertedState))
            {
                var result = _rules.GetGameResult(convertedState);
                if(result.GetWinner() == 0)
                {
                    return double.MaxValue;
                }
                else
                {
                    return double.MinValue;
                }
            }
            foreach(var square in convertedState.GetBoard().GetAllSquares())
            {
                score += CalculatePieceValue(square);
                score += CalculatePieceCoverage(convertedState, square);
            }
            return score;
        }

        private int CalculatePieceValue(BoardSquare square)
        {
            if(square.GetPiece() != null)
            {
                if(square.GetPiece().GetPlayer() == 0)
                {
                    return square.GetPiece().GetValue();
                }
                else
                {
                    return -square.GetPiece().GetValue();
                }
            }
            return 0;
        }
        
        private double CalculatePieceCoverage(StandardBoardState state, BoardSquare square)
        {
            const double COVERAGE_MULTIPLIER = 0.1;
            if(square.GetPiece() != null)
            {
                double result = 0;
                var moves = state.GetNonBlockedMoves(square);
                foreach(var move in moves)
                {
                    if(move.To.GetPiece() == null)
                    {
                        result += COVERAGE_MULTIPLIER;
                    }
                    else if(move.To.GetPiece().GetPlayer() != square.GetPiece().GetPlayer())
                    {
                        result += COVERAGE_MULTIPLIER * move.To.GetPiece().GetValue();
                    }
                }
                if (square.GetPiece().GetPlayer() == 0)
                {
                    return result;
                }
                else
                {
                    return -result;
                }
            }
            return 0;
        }

    }
}
