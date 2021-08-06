using ChessCore.Modes;
using System;

namespace ChessCore
{
    class Minimax
    {
        private IGameRules _rules;
        private BoardState _startingState;
        private int _maxDepth = 2;

        public Minimax(IGameRules rules, BoardState startingState, int maxDepth = 2)
        {
            _rules = rules;
            _startingState = startingState;
            _maxDepth = maxDepth;
        }

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
                if (gameResult == GameResult.WHITE_WIN)
                {
                    isGameOver = true;
                    winner = 0;
                }
                else if (gameResult == GameResult.BLACK_WIN)
                {
                    isGameOver = true;
                    winner = 1;
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

                if (move.Options != null)
                {
                    foreach (var option in move.Options)
                    {
                        if (!bestMoveFound)
                        {
                            move.SelectOption(option);
                            processMove();
                        }
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
