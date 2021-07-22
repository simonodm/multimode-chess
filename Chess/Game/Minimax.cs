using Chess.Game.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class Minimax
    {
        public static MinimaxResult GetBoardScore(IGameRules rules, BoardState state, int depth = 2)
        {
            int player = 0;
            if(state.GetLastMove() != null && state.GetLastMove().Piece.GetPlayer() == 0)
            {
                player = 1;
            }

            return ProcessBoard(rules, state, depth, player);
        }

        private static MinimaxResult ProcessBoard(
            IGameRules rules,
            BoardState state,
            int depth,
            int player,
            double alpha = double.MinValue,
            double beta = double.MaxValue)
        {
            if (depth == 0)
            {
                double score = rules.GetEvaluator().GetBoardScore(state);
                return new MinimaxResult(state, score);
            }

            double bestScore = player == 0 ? double.MinValue : double.MaxValue;
            Move bestMove = null;

            double newAlpha = alpha;
            double newBeta = beta;

            var legalMoves = rules.GetAllLegalMoves(state, player);
            foreach(var move in legalMoves)
            {
                if(move.IsUserInputRequired)
                {
                    move.SelectOption(0); // TODO: process for each option
                }
                var score = ProcessBoard(rules, rules.Move(move), depth - 1, (player + 1) % 2, newAlpha, newBeta);
                if(player == 0)
                {
                    if(score.Score > bestScore)
                    {
                        bestScore = score.Score;
                        bestMove = move;
                    }
                    newAlpha = Math.Max(bestScore, newAlpha);
                    if(newAlpha >= newBeta)
                    {
                        break;
                    }
                }
                else
                {
                    if(score.Score < bestScore)
                    {
                        bestScore = score.Score;
                        bestMove = move;
                    }
                    newBeta = Math.Min(bestScore, newBeta);
                    if(newAlpha >= newBeta)
                    {
                        break;
                    }
                }
            }

            return new MinimaxResult(state, bestScore, bestMove);
        }
    }
}
