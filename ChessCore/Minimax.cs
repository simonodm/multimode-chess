using ChessCore.Modes;
using System;

namespace ChessCore
{
    /// <summary>
    /// Represents a single minimax evaluation of the supplied board state according to the supplied rules and depth.
    /// </summary>
    internal class Minimax
    {
        private readonly IGameRules _rules;
        private readonly BoardState _startingState;
        private readonly int _maxDepth;

        public Minimax(IGameRules rules, BoardState startingState, int maxDepth = 2)
        {
            _rules = rules;
            _startingState = startingState;
            _maxDepth = maxDepth;
        }

        /// <summary>
        /// Evaluates the state supplied in constructor.
        /// </summary>
        /// <returns>The evaluation result</returns>
        public MinimaxResult Evaluate()
        {
            int player = 0;
            if (_startingState.GetLastMove() != null && _startingState.GetLastMove().Piece.GetPlayer() == 0)
            {
                player = 1;
            }

            return ProcessBoard(_startingState, player);
        }

        private MinimaxResult ProcessBoard(
            BoardState state,
            int player,
            int currentDepth = 0,
            double alpha = double.MinValue,
            double beta = double.MaxValue)
        {
            var gameResult = _rules.GetGameResult(state);

            // Base condition
            if (currentDepth == _maxDepth || gameResult != GameResult.ONGOING)
            {
                double score = _rules.GetEvaluator().GetBoardScore(state);
                bool isGameOver = false;
                int winner = 0;
                switch (gameResult)
                {
                    case GameResult.WHITE_WIN:
                        isGameOver = true;
                        winner = 0;
                        break;
                    case GameResult.BLACK_WIN:
                        isGameOver = true;
                        winner = 1;
                        break;
                }
                return new MinimaxResult(state, score, null, isGameOver, winner);
            }

            Move bestMove = null;
            double bestScore = player == 0 ? double.MinValue : double.MaxValue;
            double newAlpha = alpha;
            double newBeta = beta;
            bool bestMoveFound = false;

            // Branching
            var legalMoves = _rules.GetAllLegalMoves(state, player);
            foreach (var move in legalMoves)
            {
                var processMove = new Action(() =>
                {
                    if (bestMove == null)
                    {
                        bestMove = move; // preventing situations when no move is viable (no score gets below/above double.MinValue/double.MaxValue)
                    }
                    var score = ProcessBoard(_rules.Move(move), (player + 1) % 2, currentDepth + 1, newAlpha, newBeta);
                    if (player == 0)
                    {
                        if (score.Score > bestScore)
                        {
                            bestScore = score.Score;
                            bestMove = move;
                        }
                        newAlpha = Math.Max(bestScore, newAlpha);
                        if (newAlpha >= newBeta)
                        {
                            bestMoveFound = true;
                        }
                    }
                    else
                    {
                        if (score.Score < bestScore)
                        {
                            bestScore = score.Score;
                            bestMove = move;
                        }
                        newBeta = Math.Min(bestScore, newBeta);
                        if (newAlpha >= newBeta)
                        {
                            bestMoveFound = true;
                        }
                    }
                });

                if (move.IsUserInputRequired)
                {
                    foreach (var option in move.Options)
                    {
                        if (bestMoveFound) continue;
                        if (move.SelectedOption != null)
                        {
                            move.UnselectOption();
                        }

                        move.SelectOption(option);
                        processMove();
                    }
                }
                else
                {
                    processMove();
                }

                if (bestMoveFound)
                {
                    break;
                }
            }

            bool isGameOverForced = Math.Abs(bestScore) == double.MaxValue;
            int forcedGameOverWinner = bestScore > 0 ? 0 : 1;

            return new MinimaxResult(state, bestScore, bestMove, isGameOverForced, forcedGameOverWinner);
        }
    }
}
