using System.Collections.Generic;

namespace ChessCore.Modes
{
    /// <summary>
    /// Represents an interface for a game mode (e. g. standard chess or Pawn of the Dead).
    /// </summary>
    public interface IGameRules
    {
        /// <summary>
        /// Processes the supplied move. Takes move's BoardBefore as the current state.
        /// </summary>
        /// <param name="move">Move to process</param>
        /// <returns>Next board state</returns>
        public BoardState Move(Move move);

        /// <summary>
        /// Retrieves all legal moves for the supplied state and player.
        /// </summary>
        /// <param name="state">Board state to calculate legal moves for</param>
        /// <param name="player">Player to move</param>
        /// <returns>An IReadOnlyList containing all the legal moves for the given player.</returns>
        public IReadOnlyList<Move> GetAllLegalMoves(BoardState state, int player);

        /// <summary>
        /// Retrieves legal moves for the supplied state and player from the supplied square.
        /// </summary>
        /// <param name="square">Square to calculate legal moves from</param>
        /// <param name="state">Board state to calculate legal moves for</param>
        /// <param name="player">Player to move</param>
        /// <returns>An IReadOnlyList containing the legal moves for the given square and player.</returns>
        public IReadOnlyList<Move> GetLegalMoves(BoardSquare square, BoardState state, int player);

        /// <summary>
        /// Generates the initial BoardState.
        /// </summary>
        /// <returns>Initial board state</returns>
        public BoardState GetStartingBoardState();

        /// <summary>
        /// Generates the initial BoardState from the supplied board.
        /// </summary>
        /// <param name="board">Board to start with</param>
        /// <returns>Initial board state</returns>
        public BoardState GetStartingBoardState(Board board);

        /// <summary>
        /// Retrieves the game result for the supplied state.
        /// </summary>
        /// <param name="state">Board state to calculate game result for</param>
        /// <returns>The game result for the supplied state</returns>
        public GameResult GetGameResult(BoardState state);

        /// <summary>
        /// Retrieves the game mode's position evaluator.
        /// </summary>
        /// <returns>An IBoardEvaluator instance</returns>
        public IBoardEvaluator GetEvaluator();

        /// <summary>
        /// Retrieves the game mode's piece factory.
        /// </summary>
        /// <returns>An IPieceFactory instance</returns>
        public IPieceFactory GetPieceFactory();

        /// <summary>
        /// Generates the supplied move's notation.
        /// </summary>
        /// <param name="move">Move to generate notation for</param>
        /// <returns>A string representing the move in the game mode's notation.</returns>
        public string GetMoveNotation(Move move);
    }
}
