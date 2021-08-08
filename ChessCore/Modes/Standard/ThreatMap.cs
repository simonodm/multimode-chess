namespace ChessCore.Modes.Standard
{
    internal class ThreatMap
    {
        readonly int[,] _whiteThreatMap;
        readonly int[,] _blackThreatMap;

        /// <summary>
        /// Generates a map of threatened squares by both players.
        /// </summary>
        /// <param name="state">Board state to generate threats for</param>
        public ThreatMap(StandardBoardState state)
        {
            int width = state.GetBoard().GetWidth();
            int height = state.GetBoard().GetHeight();

            _whiteThreatMap = new int[width, height];
            _blackThreatMap = new int[width, height];

            GenerateThreatMaps(state);
        }

        /// <summary>
        /// Retrieves the amount of pieces by the supplied player threatening the square at the supplied file and rank.
        /// </summary>
        /// <param name="file">Square file</param>
        /// <param name="rank">Square rank</param>
        /// <param name="byPlayer">Which player to calculate threats from</param>
        /// <returns>Threat count for the given square by the given player</returns>
        public int GetThreatCount(int file, int rank, int byPlayer)
        {
            return byPlayer == 0 ? _whiteThreatMap[file, rank] : _blackThreatMap[file, rank];
        }

        /// <summary>
        /// Retrieves the amount of pieces by the supplied player threatening the square at the supplied file and rank.
        /// </summary>
        /// <param name="square">Square</param>
        /// <param name="byPlayer">Which player to calculate threats from</param>
        /// <returns>Threat count for the given square by the given player</returns>
        public int GetThreatCount(BoardSquare square, int byPlayer)
        {
            return GetThreatCount(square.GetFile(), square.GetRank(), byPlayer);
        }

        private void GenerateThreatMaps(StandardBoardState state)
        {
            foreach (var square in state.GetBoard().GetAllSquares())
            {
                ProcessSquareThreats(state, square);
            }
        }

        private void ProcessSquareThreats(StandardBoardState state, BoardSquare square)
        {
            var piece = square.GetPiece();
            if (piece == null || piece is not StandardPiece) return;

            var standardPiece = (StandardPiece)piece;
            var threatenedSquares = standardPiece.GetThreatenedSquares(state, square);

            foreach (var threatenedSquare in threatenedSquares)
            {
                IncrementSquareThreat(standardPiece, threatenedSquare);
            }
        }

        private void IncrementSquareThreat(StandardPiece piece, BoardSquare threatenedSquare)
        {
            if (piece.GetPlayer() == 0)
            {
                _whiteThreatMap[threatenedSquare.GetFile(), threatenedSquare.GetRank()]++;
            }
            else
            {
                _blackThreatMap[threatenedSquare.GetFile(), threatenedSquare.GetRank()]++;
            }
        }
    }
}
