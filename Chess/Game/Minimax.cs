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
        public static double GetBoardScore(IGameRules rules, BoardState state, int depth = 3)
        {
            int player = 0;
            if(state.LastMove != null && state.LastMove.Piece.Player == 0)
            {
                player = 1;
            }

            return ProcessBoard(rules, state, depth, player);
        }

        private static double ProcessBoard(
            IGameRules rules,
            BoardState state,
            int depth,
            int player,
            double alpha = double.MinValue,
            double beta = double.MaxValue)
        {
            if (depth == 0)
            {
                return rules.GetEvaluator().GetBoardScore(state);
            }

            double bestScore = player == 0 ? double.MinValue : double.MaxValue;

            double newAlpha = alpha;
            double newBeta = beta;

            var legalMoves = rules.GetAllLegalMoves(state, player);
            foreach(var move in legalMoves)
            {
                var score = ProcessBoard(rules, rules.Move(move), depth - 1, (player + 1) % 2, newAlpha, newBeta);
                if(player == 0)
                {
                    bestScore = Math.Max(score, bestScore);
                    newAlpha = Math.Max(bestScore, newAlpha);
                    if(newAlpha >= newBeta)
                    {
                        break;
                    }
                }
                else
                {
                    bestScore = Math.Min(score, bestScore);
                    newBeta = Math.Min(bestScore, newBeta);
                    if(newAlpha >= newBeta)
                    {
                        break;
                    }
                }
            }

            return bestScore;
        }
    }
}
