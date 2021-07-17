using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Modes;

namespace Chess.Game
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
            if(_rules.IsGameOver(state))
            {
                var result = _rules.GetGameResult(state);
                if(result.GetWinner() == 0)
                {
                    return double.MaxValue;
                }
                else
                {
                    return double.MinValue;
                }
            }
            foreach(var square in state.GetAllSquares())
            {
                score += CalculatePieceValue(state, square);
                score += CalculatePieceCoverage(state, square);
            }
            return score;
        }

        private int CalculatePieceValue(BoardState state, BoardSquare square)
        {
            if(square.Piece != null)
            {
                if(square.Piece.Player == 0)
                {
                    return square.Piece.Value;
                }
                else
                {
                    return -square.Piece.Value;
                }
            }
            return 0;
        }
        
        private double CalculatePieceCoverage(BoardState state, BoardSquare square)
        {
            const double COVERAGE_MULTIPLIER = 0.1;
            if(square.Piece != null)
            {
                double result = 0;
                var moves = _rules.GetLegalMoves(square, state);
                foreach(var move in moves)
                {
                    if(move.To.Piece == null)
                    {
                        result += COVERAGE_MULTIPLIER;
                    }
                    else if(move.To.Piece.Player != square.Piece.Player)
                    {
                        result += COVERAGE_MULTIPLIER * move.To.Piece.Value;
                    }
                }
                if (square.Piece.Player == 0)
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
